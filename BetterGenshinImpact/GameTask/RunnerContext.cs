﻿using BetterGenshinImpact.GameTask.AutoFight.Model;
using BetterGenshinImpact.Model;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using BetterGenshinImpact.GameTask.Common.Job;
using Wpf.Ui.Controls;
using static BetterGenshinImpact.GameTask.Common.TaskControl;

namespace BetterGenshinImpact.GameTask;

/// <summary>
/// 使用 TaskRunner 运行任务时的上下文
/// </summary>
public class RunnerContext : Singleton<RunnerContext>
{
    /// <summary>
    /// 当前使用队伍名称
    /// 游戏内定义的队伍名称
    /// </summary>
    public string? PartyName { get; set; }

    /// <summary>
    /// 当前队伍角色信息
    /// </summary>
    private CombatScenes? _combatScenes;

    public async Task<CombatScenes?> GetCombatScenes(CancellationToken ct)
    {
        if (_combatScenes == null)
        {
            // 返回主界面再识别
            var returnMainUiTask = new ReturnMainUiTask();
            await returnMainUiTask.Start(ct);

            await Delay(200, ct);

            _combatScenes = new CombatScenes().InitializeTeam(CaptureToRectArea());
            if (!_combatScenes.CheckTeamInitialized())
            {
                Logger.LogError("队伍角色识别失败");
                _combatScenes = null;
            }
        }
        return _combatScenes;
    }

    public void ClearCombatScenes()
    {
        _combatScenes = null;
    }

    public void Clear()
    {
        PartyName = null;
        _combatScenes = null;
    }
}
