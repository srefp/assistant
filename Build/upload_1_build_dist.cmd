cd /d %~dp0
if exist dist rd /s /q dist
mkdir dist\Assistant

@echo [prepare compiler]
for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -property installationPath`) do set "path=%path%;%%i\MSBuild\Current\Bin;%%i\Common7\IDE"

@echo [prepare version]
cd /d ..\BetterGenshinImpact
set "script=Get-Content 'BetterGenshinImpact.csproj' | Select-String -Pattern 'AssemblyVersion\>(.*)\<\/AssemblyVersion' | ForEach-Object { $_.Matches.Groups[1].Value }"
for /f "usebackq delims=" %%i in (`powershell -NoLogo -NoProfile -Command "%script%"`) do set version=%%i
echo current version is %version%
if "%b%"=="" ( set "b=%version%" )

set "tmpfolder=%~dp0dist\Assistant"
set "archiveFile=Assistant_v%b%.7z"
set "setupFile=Assistant_Setup_v%b%.exe"

echo [build app using vs2022]
cd /d %~dp0
rd /s /q ..\BetterGenshinImpact\bin\x64\Release\net8.0-windows10.0.22621.0\publish\win-x64\
cd ..\
dotnet publish -c Release -p:PublishProfile=FolderProfile

echo [pack app using 7z]
cd /d %~dp0
cd /d ..\BetterGenshinImpact\bin\x64\Release\net8.0-windows10.0.22621.0\publish\win-x64\
xcopy * "%tmpfolder%" /E /C /I /Y
cd /d %~dp0
del /f /q %tmpfolder%\*.lib
del /f /q %tmpfolder%\*ffmpeg*.dll

:: ���һЩ�����ļ���ʼ�����ļ����ʺϷ���Github��
if exist "E:\HuiTask\AssistantBuild\Assistant" (
    xcopy "E:\HuiTask\AssistantBuild\Assistant\*" "%tmpfolder%" /E /C /I /Y
)
:: ���һЩ�����ļ�����

@pause
