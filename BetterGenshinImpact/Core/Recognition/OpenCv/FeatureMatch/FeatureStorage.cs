﻿using BetterGenshinImpact.Core.Config;
using OpenCvSharp;
using System.Diagnostics;
using System.IO;

namespace BetterGenshinImpact.Core.Recognition.OpenCv.FeatureMatch;

public class FeatureStorage(string name)
{
    private readonly string rootPath = Global.Absolute(@"Assets\Map\");

    public void SetType(Feature2DType type)
    {
        TypeName = type.ToString();
    }

    public string TypeName { get; set; } = "UNKNOWN";

    // public KeyPoint[]? LoadKeyPointArray1()
    // {
    //     CreateFolder();
    //     string kpPath = Path.Combine(rootPath, $"{name}_{TypeName}.kp");
    //     if (File.Exists(kpPath))
    //     {
    //         return ObjectUtils.Deserialize(File.ReadAllBytes(kpPath)) as KeyPoint[];
    //     }
    //     return null;
    // }
    //
    // public void SaveKeyPointArray1(KeyPoint[] kpArray)
    // {
    //     CreateFolder();
    //     string kpPath = Path.Combine(rootPath, $"{name}_{TypeName}.kp");
    //     File.WriteAllBytes(kpPath, ObjectUtils.Serialize(kpArray));
    // }

    public KeyPoint[]? LoadKeyPointArray()
    {
        CreateFolder();
        string kpPath = Path.Combine(rootPath, $"{name}_{TypeName}.kp");
        if (File.Exists(kpPath))
        {
            FileStorage fs = new(kpPath, FileStorage.Modes.Read);
            var kpArray = fs["kp"]?.ReadKeyPoints();
            fs.Release();
            return kpArray;
        }
        return null;
    }

    public void SaveKeyPointArray(KeyPoint[] kpArray)
    {
        CreateFolder();
        string kpPath = Path.Combine(rootPath, $"{name}_{TypeName}.kp");
        FileStorage fs = new(kpPath, FileStorage.Modes.Write);
        fs.Write("kp", kpArray);
        fs.Release();
    }

    private void CreateFolder()
    {
        if (Directory.Exists(rootPath) == false)
        {
            Directory.CreateDirectory(rootPath);
        }
    }

    // public Mat? LoadDescMat1()
    // {
    //     CreateFolder();
    //     // 格式: Surf_336767x128.mat
    //     var files = Directory.GetFiles(rootPath, $"{name}_{TypeName}_*.mat", SearchOption.AllDirectories);
    //     if (files.Length == 0)
    //     {
    //         return null;
    //     }
    //     else if (files.Length > 1)
    //     {
    //         Debug.WriteLine($"[FeatureSerializer] Found multiple files: {string.Join(", ", files)}");
    //     }
    //     var rowColPair = Path.GetFileNameWithoutExtension(files[0])
    //         .Replace($"{name}_{TypeName}_", "")
    //         .Split('x');
    //     if (rowColPair.Length != 2)
    //     {
    //         Debug.WriteLine($"[FeatureSerializer] Invalid file name: {files[0]}");
    //         return null;
    //     }
    //     GCHandle pinnedArray = GCHandle.Alloc(ObjectUtils.Deserialize(File.ReadAllBytes(files[0])), GCHandleType.Pinned);
    //     IntPtr pointer = pinnedArray.AddrOfPinnedObject();
    //     return new Mat(Convert.ToInt32(rowColPair[0]), Convert.ToInt32(rowColPair[1]), MatType.CV_32FC1, pointer);
    // }

    public Mat? LoadDescMat()
    {
        CreateFolder();
        var files = Directory.GetFiles(rootPath, $"{name}_{TypeName}.mat", SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            return null;
        }
        else if (files.Length > 1)
        {
            Debug.WriteLine($"[FeatureSerializer] Found multiple files: {string.Join(", ", files)}");
        }
        FileStorage fs = new(files[0], FileStorage.Modes.Read);
        var mat = fs["desc"]?.ReadMat();
        fs.Release();
        return mat;
    }

    // public void SaveDescMat1(Mat descMat)
    // {
    //     CreateFolder();
    //     // 删除旧文件
    //     var files = Directory.GetFiles(rootPath, $"{name}_{TypeName}_*.mat", SearchOption.AllDirectories);
    //     foreach (var file in files)
    //     {
    //         File.Delete(file);
    //     }
    //
    //     var descPath = Path.Combine(rootPath, $"{name}_{TypeName}_{descMat.Rows}x{descMat.Cols}.mat");
    //     var bytes = new byte[descMat.Step(0) * descMat.Rows]; // matSrcRet.Total() * matSrcRet.ElemSize()
    //     Marshal.Copy(descMat.Data, bytes, 0, bytes.Length);
    //     File.WriteAllBytes(descPath, ObjectUtils.Serialize(bytes));
    // }

    public void SaveDescMat(Mat descMat)
    {
        CreateFolder();
        // 删除旧文件
        var fileName = $"{name}_{TypeName}.mat";
        var files = Directory.GetFiles(rootPath, fileName, SearchOption.AllDirectories);
        foreach (var file in files)
        {
            File.Delete(file);
        }

        var descPath = Path.Combine(rootPath, fileName);
        FileStorage fs = new(descPath, FileStorage.Modes.Write);
        fs.Write("desc", descMat);
        fs.Release();
    }
}
