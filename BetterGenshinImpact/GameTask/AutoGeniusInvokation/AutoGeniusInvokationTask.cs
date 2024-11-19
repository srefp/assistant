﻿using System.Threading;
using System.Threading.Tasks;

namespace BetterGenshinImpact.GameTask.AutoGeniusInvokation;

public class AutoGeniusInvokationTask(GeniusInvokationTaskParam taskParam) : ISoloTask
{
    public string Name => "自动七圣召唤";

    public Task Start(CancellationToken ct)
    {
        // 读取策略信息
        var duel = ScriptParser.Parse(taskParam.StrategyContent);
        duel.Run(ct);
        return Task.CompletedTask;
    }
}
