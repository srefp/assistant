using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.Core.Recorder;
using BetterGenshinImpact.Core.Script;
using BetterGenshinImpact.GameTask;
using BetterGenshinImpact.GameTask.AutoFight;
using BetterGenshinImpact.GameTask.AutoPathing;
using BetterGenshinImpact.GameTask.AutoTrackPath;
using BetterGenshinImpact.GameTask.Common;
using BetterGenshinImpact.GameTask.Common.BgiVision;
using BetterGenshinImpact.GameTask.Common.Job;
using BetterGenshinImpact.GameTask.Macro;
using BetterGenshinImpact.GameTask.QucikBuy;
using BetterGenshinImpact.GameTask.QuickSereniteaPot;
using BetterGenshinImpact.Helpers;
using BetterGenshinImpact.Helpers.Extensions;
using BetterGenshinImpact.Model;
using BetterGenshinImpact.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using HotKeySettingModel = BetterGenshinImpact.Model.HotKeySettingModel;

namespace BetterGenshinImpact.ViewModel.Pages;

public partial class HotKeyPageViewModel : ObservableObject, IViewModel
{
    private readonly ILogger<HotKeyPageViewModel> _logger;
    private readonly TaskSettingsPageViewModel _taskSettingsPageViewModel;
    public AllConfig Config { get; set; }

    [ObservableProperty] private ObservableCollection<HotKeySettingModel> _hotKeySettingModels = [];

    public HotKeyPageViewModel(IConfigService configService, ILogger<HotKeyPageViewModel> logger,
        TaskSettingsPageViewModel taskSettingsPageViewModel)
    {
        _logger = logger;
        _taskSettingsPageViewModel = taskSettingsPageViewModel;
        // 获取配置
        Config = configService.Get();

        // 构建快捷键配置列表
        BuildHotKeySettingModelList();

        var list = GetAllNonDirectoryHotkey(HotKeySettingModels);
        foreach (var hotKeyConfig in list)
        {
            hotKeyConfig.RegisterHotKey();
            hotKeyConfig.PropertyChanged += (sender, e) =>
            {
                if (sender is HotKeySettingModel model)
                {
                    // 反射更新配置

                    // 更新快捷键
                    if (e.PropertyName == "HotKey")
                    {
                        Debug.WriteLine($"{model.FunctionName} 快捷键变更为 {model.HotKey}");
                        var pi = Config.HotKeyConfig.GetType().GetProperty(model.ConfigPropertyName,
                            BindingFlags.Public | BindingFlags.Instance);
                        if (null != pi && pi.CanWrite)
                        {
                            var str = model.HotKey.ToString();
                            if (str == "< None >")
                            {
                                str = "";
                            }

                            pi.SetValue(Config.HotKeyConfig, str, null);
                        }
                    }

                    // 更新快捷键类型
                    if (e.PropertyName == "HotKeyType")
                    {
                        Debug.WriteLine($"{model.FunctionName} 快捷键类型变更为 {model.HotKeyType.ToChineseName()}");
                        model.HotKey = HotKey.None;
                        var pi = Config.HotKeyConfig.GetType().GetProperty(model.ConfigPropertyName + "Type",
                            BindingFlags.Public | BindingFlags.Instance);
                        if (null != pi && pi.CanWrite)
                        {
                            pi.SetValue(Config.HotKeyConfig, model.HotKeyType.ToString(), null);
                        }
                    }

                    RemoveDuplicateHotKey(model);
                    model.UnRegisterHotKey();
                    model.RegisterHotKey();
                }
            };
        }
    }

    /// <summary>
    /// 移除重复的快捷键配置
    /// </summary>
    /// <param name="current"></param>
    private void RemoveDuplicateHotKey(HotKeySettingModel current)
    {
        if (current.HotKey.IsEmpty)
        {
            return;
        }

        var list = GetAllNonDirectoryHotkey(HotKeySettingModels);
        foreach (var hotKeySettingModel in list)
        {
            if (hotKeySettingModel.HotKey.IsEmpty)
            {
                continue;
            }

            if (hotKeySettingModel.ConfigPropertyName != current.ConfigPropertyName &&
                hotKeySettingModel.HotKey == current.HotKey)
            {
                hotKeySettingModel.HotKey = HotKey.None;
            }
        }
    }

    public static List<HotKeySettingModel> GetAllNonDirectoryHotkey(IEnumerable<HotKeySettingModel> modelList)
    {
        var list = new List<HotKeySettingModel>();
        foreach (var hotKeySettingModel in modelList)
        {
            if (!hotKeySettingModel.IsDirectory)
            {
                list.Add(hotKeySettingModel);
            }

            list.AddRange(GetAllNonDirectoryChildren(hotKeySettingModel));
        }

        return list;
    }

    public static List<HotKeySettingModel> GetAllNonDirectoryChildren(HotKeySettingModel model)
    {
        var result = new List<HotKeySettingModel>();

        if (model.Children.Count == 0)
        {
            return result;
        }

        foreach (var child in model.Children)
        {
            if (!child.IsDirectory)
            {
                result.Add(child);
            }

            // 递归调用以获取子节点中的非目录对象
            result.AddRange(GetAllNonDirectoryChildren(child));
        }

        return result;
    }

    private void BuildHotKeySettingModelList()
    {
        // 一级目录/快捷键
        var bgiEnabledHotKeySettingModel = new HotKeySettingModel(
            "启动停止 耕地机",
            nameof(Config.HotKeyConfig.BgiEnabledHotkey),
            Config.HotKeyConfig.BgiEnabledHotkey,
            Config.HotKeyConfig.BgiEnabledHotkeyType,
            (_, _) =>
            {
                WeakReferenceMessenger.Default.Send(
                    new PropertyChangedMessage<object>(this, "SwitchTriggerStatus", "", ""));
            }
        );
        HotKeySettingModels.Add(bgiEnabledHotKeySettingModel);

        var autoTpDirectory = new HotKeySettingModel(
            "自动传送"
        );
        HotKeySettingModels.Add(autoTpDirectory);
        
        // 二级快捷键
        autoTpDirectory.Children.Add(new HotKeySettingModel(
            "自动传送下一个点位",
            nameof(Config.HotKeyConfig.AutoTpNextHotkey),
            Config.HotKeyConfig.AutoTpNextHotkey,
            Config.HotKeyConfig.AutoTpNextHotkeyType,
            (_, _) => { TaskTriggerDispatcher.TpNext(); }
        ));

        autoTpDirectory.Children.Add(new HotKeySettingModel(
            "自动传送上一个点位",
            nameof(Config.HotKeyConfig.AutoTpPrevHotkey),
            Config.HotKeyConfig.AutoTpPrevHotkey,
            Config.HotKeyConfig.AutoTpPrevHotkeyType,
            (_, _) => { TaskTriggerDispatcher.TpPrev(); }
        ));
        
        autoTpDirectory.Children.Add(new HotKeySettingModel(
            "qm自动传送下一个点位",
            nameof(Config.HotKeyConfig.AutoQmTpNextHotkey),
            Config.HotKeyConfig.AutoQmTpNextHotkey,
            Config.HotKeyConfig.AutoQmTpNextHotkeyType,
            (_, _) => { TaskTriggerDispatcher.QmTpNext(); }
        ));
    }

    private void SwitchSoloTask(IAsyncRelayCommand asyncRelayCommand)
    {
        if (asyncRelayCommand.IsRunning)
        {
            CancellationContext.Instance.Cancel();
        }
        else
        {
            asyncRelayCommand.Execute(null);
        }
    }

    private string ToChinese(bool enabled)
    {
        return enabled.ToChinese();
    }
}