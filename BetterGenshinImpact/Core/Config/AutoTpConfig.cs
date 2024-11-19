using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Core.Config;

/// <summary>
///     自动传送配置
/// </summary>
[Serializable]
public partial class AutoTpConfig : ObservableObject
{
    /// <summary>
    ///     讨伐按钮位置
    /// </summary>
    [ObservableProperty] private string _crusadePos;
    /// <summary>
    ///     按钮延迟
    /// </summary>
    [ObservableProperty] private int _buttonDelay;

    /// <summary>
    ///     路线
    /// </summary>
    [ObservableProperty] private string _route;

    /// <summary>
    /// 自动吃药
    /// </summary>
    [ObservableProperty] private bool _autoFoodEnabled = true;

    /// <summary>
    /// 自动传送
    /// </summary>
    [ObservableProperty] private bool _autoTpEnabled = true;

    /// <summary>
    /// 其他工具
    /// </summary>
    [ObservableProperty] private bool _otherToolEnabled = true;

    /// <summary>
    ///     当前位置
    /// </summary>
    [ObservableProperty] private string _currentPosition;

    /// <summary>
    ///     传送模式
    /// </summary>
    [ObservableProperty] private string _tpMode;

    /// <summary>
    ///     全局qm
    /// </summary>
    [ObservableProperty] private bool _globalQm;

    /// <summary>
    ///     全局自动捡材料
    /// </summary>
    [ObservableProperty] private string _globalAutoPick;

    /// <summary>
    ///     长按F持续捡材料
    /// </summary>
    [ObservableProperty] private string _fAutoPick;

    /// <summary>
    ///     半自动传送
    /// </summary>
    [ObservableProperty] private string _halfAutoTp;

    /// <summary>
    ///     自动冲刺模式
    /// </summary>
    [ObservableProperty] private string _runMode;

    /// <summary>
    /// 全局qm是否启用
    /// </summary>
    [ObservableProperty] private bool _globalTpEnabled = true;

    /// <summary>
    /// 全局自动捡材料是否启用
    /// </summary>
    [ObservableProperty] private bool _globalAutoPickEnabled = true;

    /// <summary>
    /// 长按F自动捡材料是否启用
    /// </summary>
    [ObservableProperty] private bool _fAutoPickEnabled = true;

    /// <summary>
    /// 半自动传送
    /// </summary>
    [ObservableProperty] private bool _halfAutoTpEnabled = true;

    /// <summary>
    /// 吃药坐标列表
    /// </summary>
    [ObservableProperty] private string _foodCoords = "";

    /// <summary>
    /// 路线文件夹
    /// </summary>
    [ObservableProperty] private string _routesDir = "";

    /// <summary>
    /// 禁止传送
    /// </summary>
    [ObservableProperty] private bool _tpForbidden;

    /// <summary>
    /// 当前路线位置
    /// </summary>
    [ObservableProperty] private int _routeIndex;
}