using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Assistant.Engine.Executor;
using BetterGenshinImpact.Assistant.Bean;
using BetterGenshinImpact.Assistant.Engine.Parser;
using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.GameTask;
using BetterGenshinImpact.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BetterGenshinImpact.ViewModel.Pages;

public partial class AutoTpPageViewModel : ObservableObject, INavigationAware, IViewModel
{
    public AllConfig Config { get; set; }

    [ObservableProperty] private List<string> _routes = [];

    [ObservableProperty] private ObservableCollection<string> _startPositions = [];

    [ObservableProperty] private List<string> _tpModes = ["优先开图", "优先选区", "优先开书"];

    [ObservableProperty] private List<string> _runModes = ["关闭", "匀速冲刺", "跑跳"];

    [ObservableProperty] private int _titleFontSize = 16;

    [ObservableProperty] private int _subtitleFontSize = 14;

    [ObservableProperty] private int _comboBoxMinWidth = 160;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(StartTriggerCommand))]
    private bool _startButtonEnabled = true;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(StopTriggerCommand))]
    private bool _stopButtonEnabled = true;

    private List<TpPoint> _tpPoints = [];

    // 记录上次使用原神的句柄
    private IntPtr _hWnd;

    public AutoTpPageViewModel(IConfigService configService, INavigationService navigationService)
    {
        Config = configService.Get();
        // 加载路线文件
        var autoTpConfig = Config.AutoTpConfig;
        var routePath = autoTpConfig.RoutesDir;
        if (string.IsNullOrEmpty(routePath))
        {
            return;
        }

        var routeFiles = Directory.GetFiles(routePath);
        foreach (var routeFile in routeFiles)
        {
            _routes.Add(Path.GetFileNameWithoutExtension(routeFile));
        }

        GetTpPointsFromRoute();
    }

    private void GetTpPointsFromRoute()
    {
        StartPositions.Clear();
        StartPositions.Add("不在路线中");
        if (string.IsNullOrEmpty(Config.AutoTpConfig.Route))
        {
            return;
        }

        _tpPoints = RouteUtil.ParseFile(Path.Combine(Config.AutoTpConfig.RoutesDir,
            Config.AutoTpConfig.Route + ".lua"));

        // 将路线设置到执行器中
        RouteExecutor.routes = _tpPoints;
        for (var tpIndex = 0; tpIndex < _tpPoints.Count; tpIndex++)
        {
            var tpPoint = _tpPoints[tpIndex];
            StartPositions.Add(tpPoint.Name ?? $"{tpIndex}");
        }
    }

    [RelayCommand]
    public void RouteChanged()
    {
        GetTpPointsFromRoute();
    }

    private bool CanStartTrigger() => StartButtonEnabled;

    private bool CanStopTrigger() => StopButtonEnabled;

    [RelayCommand(CanExecute = nameof(CanStartTrigger))]
    public async Task OnStartTriggerAsync()
    {
        var hWnd = SystemControl.FindGenshinImpactHandle();
        if (hWnd == IntPtr.Zero)
        {
            if (Config.GenshinStartConfig.LinkedStartEnabled &&
                !string.IsNullOrEmpty(Config.GenshinStartConfig.InstallPath))
            {
                hWnd = await SystemControl.StartFromLocalAsync(Config.GenshinStartConfig.InstallPath);
                if (hWnd != IntPtr.Zero)
                {
                    TaskContext.Instance().LinkedStartGenshinTime = DateTime.Now; // 标识关联启动原神的时间
                }
            }

            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Error("未找到原神窗口，请先启动原神！");
                return;
            }
        }

        Start(hWnd);
    }

    private void Start(IntPtr hWnd)
    {
    }

    [RelayCommand(CanExecute = nameof(CanStopTrigger))]
    private void OnStopTrigger()
    {
        Stop();
    }

    private void Stop()
    {
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }
}