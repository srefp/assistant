﻿using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.GameTask.AutoFight;
using BetterGenshinImpact.GameTask.Common;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BetterGenshinImpact.GameTask.AutoPathing.Handler;

internal class AutoFightHandler : IActionHandler
{
    public async Task RunAsync(CancellationToken ct)
    {
        await StartFight(ct);
    }

    private async Task StartFight(CancellationToken ct)
    {
        TaskControl.Logger.LogInformation("执行 {Text}", "自动战斗");
        // 爷们要战斗
        var taskParams = new AutoFightParam(GetFightStrategy())
        {
            FightFinishDetectEnabled = true,
            PickDropsAfterFightEnabled = true
        };
        var fightSoloTask = new AutoFightTask(taskParams);
        await fightSoloTask.Start(ct);
    }

    private string GetFightStrategy()
    {
        var path = Global.Absolute(@"User\AutoFight\" + TaskContext.Instance().Config.AutoFightConfig.StrategyName + ".txt");
        if ("根据队伍自动选择".Equals(TaskContext.Instance().Config.AutoFightConfig.StrategyName))
        {
            path = Global.Absolute(@"User\AutoFight\");
        }
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            throw new Exception("战斗策略文件不存在");
        }

        return path;
    }
}
