﻿using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.Core.Recorder;
using BetterGenshinImpact.Core.Script.Project;
using BetterGenshinImpact.GameTask;
using BetterGenshinImpact.GameTask.AutoPathing;
using BetterGenshinImpact.GameTask.AutoPathing.Model;
using BetterGenshinImpact.ViewModel.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterGenshinImpact.Core.Script.Group;

public partial class ScriptGroupProject : ObservableObject
{
    [ObservableProperty]
    private int _index;

    /// <summary>
    /// 理论上是文件名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 理论上是当前类型脚本根目录到脚本文件所在目录的相对路径
    /// 但是：
    /// 1. JS 脚本的文件名是内部的名称，文件夹名脚本所在文件夹，这个也是唯一标识
    /// 2. KeyMouse 脚本的文件名和文件夹名相同，文件夹名暂时无意义
    /// 3. Pathing 文件名就是实际脚本的文件名，文件夹名是脚本所在的相对目录
    /// </summary>
    public string FolderName { get; set; } = string.Empty;

    [ObservableProperty]
    private string _type = string.Empty;

    [JsonIgnore]
    public string TypeDesc => ScriptGroupProjectExtensions.TypeDescriptions[Type];

    [ObservableProperty]
    private string _status = string.Empty;

    [JsonIgnore]
    public string StatusDesc => ScriptGroupProjectExtensions.StatusDescriptions[Status];

    /// <summary>
    /// 执行周期
    /// 不在 ScheduleDescriptions 中则会被视为自定义Cron表达式
    /// </summary>
    [ObservableProperty]
    private string _schedule = string.Empty;

    [JsonIgnore]
    public string ScheduleDesc => ScriptGroupProjectExtensions.ScheduleDescriptions.GetValueOrDefault(Schedule, "自定义周期");

    [ObservableProperty]
    private int _runNum = 1;

    [JsonIgnore]
    public ScriptProject? Project { get; set; }

    public ExpandoObject? JsScriptSettingsObject { get; set; }

    /// <summary>
    /// 所属配置组
    /// </summary>
    [JsonIgnore]
    public ScriptGroup? GroupInfo { get; set; }

    public ScriptGroupProject()
    {
    }

    public ScriptGroupProject(ScriptProject project)
    {
        Name = project.Manifest.Name;
        FolderName = project.FolderName;
        Status = "Enabled";
        Schedule = "Daily";
        Project = project;
        Type = "Javascript";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="folder"></param>
    /// <param name="type">KeyMouse|Pathing</param>
    public ScriptGroupProject(string name, string folder, string type)
    {
        Name = name;
        FolderName = folder;
        Status = "Enabled";
        Schedule = "Daily";
        Project = null; // 不是JS脚本
        Type = type;
    }

    public static ScriptGroupProject BuildKeyMouseProject(string name)
    {
        return new ScriptGroupProject(name, name, "KeyMouse");
    }

    public static ScriptGroupProject BuildPathingProject(string name, string folder)
    {
        return new ScriptGroupProject(name, folder, "Pathing");
    }

    /// <summary>
    /// 通过 FolderName 查找 ScriptProject
    /// </summary>
    public void BuildScriptProjectRelation()
    {
        if (string.IsNullOrEmpty(FolderName))
        {
            throw new Exception("FolderName 为空");
        }
        Project = new ScriptProject(FolderName);
    }

    public async Task Run()
    {
        if (Type == "Javascript")
        {
            if (Project == null)
            {
                throw new Exception("JS脚本未初始化");
            }
            JsScriptSettingsObject ??= new ExpandoObject();
            await Project.ExecuteAsync(JsScriptSettingsObject);
        }
        if (Type == "KeyMouse")
        {
            // 加载并执行
            var json = await File.ReadAllTextAsync(Global.Absolute(@$"User\KeyMouseScript\{Name}"));
            await KeyMouseMacroPlayer.PlayMacro(json, CancellationContext.Instance.Cts.Token, false);
        }
        if (Type == "Pathing")
        {
            // 加载并执行
            var task = PathingTask.BuildFromFilePath(Path.Combine(MapPathingViewModel.PathJsonPath, FolderName, Name));
            TaskTriggerDispatcher.Instance().AddTrigger("AutoPick", null);
            var pathingTask = new PathExecutor(CancellationContext.Instance.Cts.Token);
            pathingTask.PartyConfig = GroupInfo?.Config.PathingConfig;
            await pathingTask.Pathing(task);
        }
        else
        {
            //throw new Exception("不支持的脚本类型");
        }
    }

    partial void OnTypeChanged(string value)
    {
        OnPropertyChanged(nameof(TypeDesc));
    }

    partial void OnStatusChanged(string value)
    {
        OnPropertyChanged(nameof(StatusDesc));
    }

    partial void OnScheduleChanged(string value)
    {
        OnPropertyChanged(nameof(ScheduleDesc));
    }
}

public class ScriptGroupProjectExtensions
{
    public static readonly Dictionary<string, string> TypeDescriptions = new()
    {
        { "Javascript", "JS脚本" },
        { "KeyMouse", "键鼠脚本" },
        { "Pathing", "路径追踪" }
    };

    public static readonly Dictionary<string, string> StatusDescriptions = new()
    {
        { "Enabled", "启用" },
        { "Disabled", "禁用" }
    };

    public Dictionary<string, string> GetStatusDescriptions()
    {
        return StatusDescriptions;
    }

    public static readonly Dictionary<string, string> ScheduleDescriptions = new()
    {
        { "Daily", "每日" },
        { "EveryTwoDays", "间隔两天" },
        { "Monday", "每周一" },
        { "Tuesday", "每周二" },
        { "Wednesday", "每周三" },
        { "Thursday", "每周四" },
        { "Friday", "每周五" },
        { "Saturday", "每周六" },
        { "Sunday", "每周日" }
    };

    public Dictionary<string, string> GetScheduleDescriptions()
    {
        return ScheduleDescriptions;
    }
}