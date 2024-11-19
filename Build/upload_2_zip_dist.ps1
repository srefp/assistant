# �����Ҫ��ģ��
Import-Module -Name Microsoft.PowerShell.Archive

# ����Ŀ¼·��
$directoryPath = ".\dist\Assistant"
$outputJsonPath = "E:\HuiTask\AssistantBuild\UploadGit\bettergi-installation-data\hash.json"
$destinationDir = "E:\HuiTask\AssistantBuild\UploadGit\bettergi-installation-data\installation"

# �����·��ת��Ϊ����·��
$absoluteDirectoryPath = (Resolve-Path -Path $directoryPath).Path

# ����Ҫ������Ŀ¼
$excludedDirectories = @(
    ".\dist\Assistant\Script",
    ".\dist\Assistant\User"
)
# �����·��ת��Ϊ����·��
$excludedDirectories = $excludedDirectories | ForEach-Object { (Resolve-Path -Path $_).Path }

# ��ʼ��һ���յĹ�ϣ�����洢�ļ�·���͹�ϣֵ
$fileHashes = @{}

# ��ȡĿ¼�µ������ļ���������Ŀ¼
$files = Get-ChildItem -Path $directoryPath -Recurse -File

foreach ($file in $files) {
    # �����Ѿ��� .zip ���ļ�
    if ($file.Extension -eq ".zip") {
        continue
    }
    # ����ļ��Ƿ���Ҫ������Ŀ¼��
    $skipFile = $false
    foreach ($excludedDir in $excludedDirectories) {
        if ($file.FullName.StartsWith($excludedDir)) {
            $skipFile = $true
            break
        }
    }
    if ($skipFile) {
        Write-Host "Skipping file in excluded directory: $($file.FullName)"
        continue
    }

    # �����ļ��Ĺ�ϣֵ
    $hash = Get-FileHash -Path $file.FullName -Algorithm SHA256

    # ����ϣֵ�Ƿ�Ϊ��
    if ($null -eq $hash) {
        Write-Host "Failed to compute hash for file: $($file.FullName)"
        continue
    }

    # �������·��
    $relativePath = $file.FullName.Replace($absoluteDirectoryPath, "").TrimStart("\\")

    # �����·���͹�ϣֵ��ӵ���ϣ����
    $fileHashes[$relativePath] = $hash.Hash

    # ����ѹ���ļ���·��
    $zipFilePath = "$($file.FullName).zip"

    # ѹ���ļ����滻ͬ��ѹ���ļ�
    Compress-Archive -Path $file.FullName -DestinationPath $zipFilePath -Force
}

# ����ϣ��ת��Ϊ JSON ��ʽ
$jsonContent = $fileHashes | ConvertTo-Json -Depth 10

# ʹ�� UTF-8 ����д�� JSON �ļ�
[System.IO.File]::WriteAllText($outputJsonPath, $jsonContent, [System.Text.Encoding]::UTF8)



# ��ȡ���� .zip �ļ���������Ŀ¼
$zipFiles = Get-ChildItem -Path $absoluteDirectoryPath -Recurse -Filter *.zip

foreach ($file in $zipFiles) {
    # ����Ŀ��·��
    $relativePath = $file.FullName.Substring($absoluteDirectoryPath.Length)
    $destinationPath = Join-Path $destinationDir $relativePath

    # ����Ŀ��Ŀ¼
    $destinationDirPath = Split-Path $destinationPath
    if (-not (Test-Path $destinationDirPath)) {
        New-Item -ItemType Directory -Path $destinationDirPath -Force
    }

    # �����ļ�
    Copy-Item -Path $file.FullName -Destination $destinationPath -Force
}

Remove-Item -Path $absoluteDirectoryPath -Recurse -Force