using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using BetterGenshinImpact;
using BetterGenshinImpact.Assistant.Bean;
using BetterGenshinImpact.Assistant.Engine.Operation;
using BetterGenshinImpact.Service;
using Core.Config;
using Microsoft.Extensions.Logging;
using Vanara.PInvoke;

namespace Assistant.Engine.Executor;

// 路线执行器
public class RouteExecutor
{
    private static readonly ILogger _logger = App.GetLogger<KeyMouseUtil>();
    public static List<TpPoint> routes = [];
    public static int[] prevBoss = [0, 0];

    public static bool sameBoss;

    // 讨伐按钮的位置
    public static int[] crusadePos = [9884, 33238];

    // 清空滚轮的位置
    public static int[] clearWheelPos = [32622, 17241];

    // 三列怪的不同x值
    public static int[] bossColumnPos = [17242, 23046, 29362];

    public static AutoTpConfig getConfig()
    {
        var config = ConfigService.Config;
        // 加载路线文件
        return config.AutoTpConfig;
    }

    public static void TpNext(bool qm)
    {
        if (!getConfig().AutoTpEnabled)
        {
            return;
        }
        var routeIndex = getConfig().RouteIndex;
        var tpForbidden = getConfig().TpForbidden;
        
        if (tpForbidden)
        {
            return;
        }

        if (routeIndex >= 0 && routeIndex <= routes.Count)
        {
            routeIndex++;
        }

        if (routeIndex > 0 && routeIndex <= routes.Count)
        {
            executeStep(routes[routeIndex - 1], qm);
        }

        // 3秒后解除冷却
        SetTimer(3000, () => tpForbidden = false);
    }
    
    public static void TpPrev(bool qm)
    {
        if (!getConfig().AutoTpEnabled)
        {
            return;
        }
        var routeIndex = getConfig().RouteIndex;
        var tpForbidden = getConfig().TpForbidden;
    
        if (tpForbidden)
        {
            return;
        }

        if (routeIndex > 0 && routeIndex <= routes.Count + 1)
        {
            routeIndex--;
            if (routeIndex == 0)
            {
                routeIndex = routes.Count;
            }
        }

        if (routeIndex > 0 && routeIndex <= routes.Count)
        {
            executeStep(routes[routeIndex - 1], false);
        }

        // 3秒后解除冷却
        SetTimer(3000, () => tpForbidden = false);
    }

    public static async void executeStep(TpPoint tpPoint, bool qmParam)
    {
        getConfig().GlobalAutoPickEnabled = true;
        var globalQm = getConfig().GlobalQm;
        int[] point = [0, 0];
        int[] selectPoint = [0, 0];
        bool qm = true;
        int wheel = 0;
        int narrow = 0;
        bool qmTp = false;
        TpMethod tpMethod = TpMethod.F2;
        int row = 0;
        int column = 0;

        // 记录开图总时间
        int totalTime = 0;

        if (tpPoint.Select != null)
        {
            selectPoint = [46917, 46641];
        }

        _logger.LogError($"路线为：{tpPoint}");

        if (tpPoint.PosD != null)
        {
            point = [tpPoint.PosD[0], tpPoint.PosD[1]];
            tpMethod = TpMethod.Direct;
        }
        else if (tpPoint is { Pos: not null, Boss: not null })
        {
            point = [tpPoint.Pos[0], tpPoint.Pos[1]];
            row = tpPoint.Boss[0];
            column = tpPoint.Boss[1];
            sameBoss = prevBoss[0] == row && prevBoss[1] == column;

            tpMethod = TpMethod.F2;
        }
        else if (tpPoint.PosA != null)
        {
            point = [tpPoint.PosA[0], tpPoint.PosA[1]];
            tpMethod = TpMethod.Area;
        }

        if (tpMethod == TpMethod.Direct && tpPoint.NarrowD != null)
        {
            narrow = tpPoint.NarrowD.Value;
        }
        else if (tpMethod == TpMethod.F2 && tpPoint.Narrow != null)
        {
            narrow = tpPoint.Narrow.Value;
        }
        else if (tpMethod == TpMethod.Area && tpPoint.NarrowA != null)
        {
            narrow = tpPoint.NarrowA.Value;
        }

        if (qmParam)
        {
            qmTp = qmParam;
        }
        else
        {
            qmTp = globalQm && qm;
        }

        // 开书
        // 开书前放Q
        if (qmTp)
        {
            KeyMouseUtil.Send(User32.VK.VK_M);
            KeyMouseUtil.ClickRight();
            KeyMouseUtil.Sleep(50);
            KeyMouseUtil.Send(User32.VK.VK_Q);
            KeyMouseUtil.Sleep(10);
        }

        if (tpMethod == TpMethod.Direct)
        {
            KeyMouseUtil.Send(User32.VK.VK_M);
            KeyMouseUtil.Sleep(OPEN_MAP_SLEEP);
        }
        else if (tpMethod == TpMethod.F2)
        {
            KeyMouseUtil.Send(User32.VK.VK_F1);
            if (crusade)
            {
                KeyMouseUtil.Sleep(BOOK_SLEEP3);
                totalTime += BOOK_SLEEP3;
            }
            else if (sameBoss)
            {
                KeyMouseUtil.Sleep(BOOK_SLEEP);
                totalTime += BOOK_SLEEP;
            }
            else
            {
                KeyMouseUtil.Sleep(BOOK_SLEEP2);
                totalTime += BOOK_SLEEP2;
            }

            // 额外等开书
            if (tpPoint.DelayBook != null)
            {
                var delay = tpPoint.DelayBook.Value;
                KeyMouseUtil.Sleep(delay);
                totalTime += delay;
            }

            // 点击讨伐
            if (crusade)
            {
                KeyMouseUtil.Click(crusadePos);
                KeyMouseUtil.Sleep(CRUSADE_SLEEP);
                totalTime += CRUSADE_SLEEP;
                crusade = false;
            }

            if (!sameBoss)
            {
                KeyMouseUtil.LongClick(clearWheelPos);
                KeyMouseUtil.Sleep(CLICK_DOWN_SLEEP);
                totalTime += CLICK_DOWN_SLEEP;

                KeyMouseUtil.Wheel((row - 1) * ROW_WHEEL_NUM);
                KeyMouseUtil.Sleep(WHEEL_SLEEP);
                totalTime += WHEEL_SLEEP;

                var bossPosX = bossColumnPos[column - 1];
                KeyMouseUtil.Click([bossPosX, bossRowPos]);
                KeyMouseUtil.Sleep(BUTTON_SLEEP);
                totalTime += BUTTON_SLEEP;
            }

            // 追踪怪
            if (sameBoss)
            {
                KeyMouseUtil.Click(trackBossPos);
                KeyMouseUtil.Sleep(CANCEL_AND_CLICK_SLEEP);
                totalTime += CANCEL_AND_CLICK_SLEEP;

                KeyMouseUtil.Click(trackBossPos);
                KeyMouseUtil.Sleep(MAP_SLEEP);
                totalTime += MAP_SLEEP;
            }
            else
            {
                KeyMouseUtil.Click(trackBossPos);
                KeyMouseUtil.Sleep(5);
                totalTime += 5;

                KeyMouseUtil.Click(trackBossPos);
                KeyMouseUtil.Sleep(MAP_SLEEP2);
                totalTime += MAP_SLEEP2;

                prevBoss[0] = row;
                prevBoss[1] = column;
            }

            // 额外等待开地图
            if (tpPoint.DelayMap != null)
            {
                KeyMouseUtil.Sleep(tpPoint.DelayMap.Value);
                totalTime += tpPoint.DelayMap.Value;
            }
        }
        else if (tpMethod == TpMethod.Area && tpPoint.Area != null)
        {
            KeyMouseUtil.Send(User32.VK.VK_M);
            KeyMouseUtil.Sleep(OPEN_MAP_SLEEP);
            totalTime += OPEN_MAP_SLEEP;

            KeyMouseUtil.Click(confirmPos);
            KeyMouseUtil.Sleep(BUTTON_SLEEP);
            totalTime += BUTTON_SLEEP;
            
            KeyMouseUtil.Click(mapArea[tpPoint.Area.Value]);
            KeyMouseUtil.Sleep(AREA_SLEEP);
            totalTime += AREA_SLEEP;
        }
        
        // 缩小
        if (narrow != 0)
        {
            if (narrow > 0)
            {
                for (var i = 0; i < narrow; i++)
                {
                    KeyMouseUtil.Click(narrowPos);
                    KeyMouseUtil.Sleep(5);
                    totalTime += 5;
                }
            }
            else
            {
                for (var i = 0; i < -narrow; i++)
                {
                    KeyMouseUtil.Click(enlargePos);
                    KeyMouseUtil.Sleep(5);
                    totalTime += 5;
                }
            }
        }

        int[] drag = [0, 0, 0, 0];
        if (tpMethod == TpMethod.F2 && tpPoint.Drag != null)
        {
            // F2拖动
            drag = tpPoint.Drag.ToArray();
        }
        else if (tpMethod == TpMethod.Direct && tpPoint.DragD != null)
        {
            // 直接开图拖动
            drag = tpPoint.DragD.ToArray();
        }
        else if (tpMethod == TpMethod.Area && tpPoint.DragA != null)
        {
            // 选区拖动
            drag = tpPoint.DragA.ToArray();
        }

        if (drag.Sum() != 0)
        {
            KeyMouseUtil.Drag(drag);
            KeyMouseUtil.Sleep(BUTTON_SLEEP);
            totalTime += BUTTON_SLEEP;
        }

        if (tpMethod == TpMethod.F2 && tpPoint.Wheel != null)
        {
            wheel = tpPoint.Wheel.Value;
        }
        if (tpMethod == TpMethod.Direct && tpPoint.WheelD != null)
        {
            wheel = tpPoint.WheelD.Value;
        }
        if (tpMethod == TpMethod.Area && tpPoint.WheelA != null)
        {
            wheel = tpPoint.WheelA.Value;
        }

        if (wheel != 0)
        {
            KeyMouseUtil.Wheel(wheel);
            KeyMouseUtil.Sleep(WHEEL_SLEEP);
            totalTime += WHEEL_SLEEP;
        }
        
        // 点击传送锚点
        KeyMouseUtil.Click(point);

        var tpDelay = BUTTON_SLEEP;
        if (tpPoint.DelayTp != null)
        {
            tpDelay = tpPoint.DelayTp.Value;
        }
        else if (selectPoint.Sum() != 0)
        {
            tpDelay = SELECT_TWO_WAIT_SLEEP;
        }
        
        KeyMouseUtil.Sleep(tpDelay);
        totalTime += tpDelay;

        if (selectPoint.Sum() != 0)
        {
            KeyMouseUtil.Click(selectPoint);
            KeyMouseUtil.Sleep(SELECT_TWO_CLICK_SLEEP);
            totalTime += SELECT_TWO_CLICK_SLEEP;
        }

        // 额外等待点击确认
        if (tpPoint.DelayConfirm != null)
        {
            KeyMouseUtil.Sleep(tpPoint.DelayConfirm.Value);
            totalTime += tpPoint.DelayConfirm.Value;
        }

        // 为了qm，补足整个延迟时间
        if (waitQmSum && qmTp && totalTime < 1200)
        {
            KeyMouseUtil.Sleep(1200 - totalTime);
        }

        KeyMouseUtil.Click(confirmPos);
        KeyMouseUtil.Sleep(BUTTON_SLEEP);
        
        // 对于可能出现地脉花的地方
        if (tpPoint.Flower != null)
        {
            KeyMouseUtil.Click(point);
            KeyMouseUtil.Sleep(SELECT_TWO_WAIT_SLEEP);
            KeyMouseUtil.Click(selectPoint);
            KeyMouseUtil.Sleep(SELECT_TWO_CLICK_SLEEP);
            KeyMouseUtil.Click(confirmPos);
            KeyMouseUtil.Sleep(BUTTON_SLEEP);
        }

        // 开始快捡
        getConfig().GlobalAutoPickEnabled = false;
    }

    // 是否等Q完
    private static bool waitQmSum = false;
    // 缩小按钮位置
    private static readonly int[] narrowPos = [1502, 39461];
    // 放大按钮位置
    private static readonly int[] enlargePos = [1502, 25923];
    // 确认按钮的位置
    private static readonly int[] confirmPos = [57546, 61225];

    // 追踪怪的位置
    private static readonly int[] trackBossPos = [49420, 51238];

    // 怪的y值
    private static readonly int bossRowPos = 20944;

    // 地图区域的标点
    private static readonly int[][] mapArea =
    [
        [49574, 10867], [59492, 11079], [49335, 17484], [59475, 17606], [49642, 23555], [58416, 23707], [49335, 30992],
        [59458, 30961], [49284, 37154]
    ];

    // 滚动一行需要的滚轮数
    private static readonly int ROW_WHEEL_NUM = 9;

    // 点击按钮的延时
    private static readonly int BUTTON_SLEEP = 80;

    // 不跨怪开书等待时间
    private static readonly int BOOK_SLEEP = 470;

    // 跨怪开书等待时间
    private static readonly int BOOK_SLEEP2 = 600;

    // 首次开书等待时间
    private static readonly int BOOK_SLEEP3 = 600;

    // 不跨怪Map等待时间
    private static readonly int MAP_SLEEP = 850;

    // 跨怪Map等待时间
    private static readonly int MAP_SLEEP2 = 900;

    // 取消后再次点击的等待时间
    private static readonly int CANCEL_AND_CLICK_SLEEP = 50;

    // 点击讨伐后的等待时间
    private static readonly int CRUSADE_SLEEP = 200;

    // 长按鼠标的等待时间
    private static readonly int CLICK_DOWN_SLEEP = 60;

    // 选怪时滚轮滚动等待时间
    private static readonly int WHEEL_SLEEP = 100;

    // 锚点双选时的等待时间
    private static readonly int SELECT_TWO_WAIT_SLEEP = 500;

    // 锚点双选时点击后的等待时间
    private static readonly int SELECT_TWO_CLICK_SLEEP = 160;

    // 快传等待时间
    private static readonly int DIRECT_TP_SLEEP = 90;

    // 快传复位等待时间
    private static readonly int DIRECT_TP_BAG_SLEEP = 80;

    // 快检等待时间，5不意味着5ms！！！
    private static readonly int QUICK_PICK_SLEEP = 5;

    // 开背包的延迟
    private static readonly int BAG_SLEEP = 550;

    // 开地图延迟
    private static readonly int OPEN_MAP_SLEEP = 550;

    // 切换地图延迟
    private static readonly int AREA_SLEEP = 200;


    private static bool crusade = true;

    public static void SetTimer(int milliSeconds, Action action)
    {
        Timer timer = new(milliSeconds);
        // 将 Action 绑定到 Elapsed 事件
        timer.Elapsed += (sender, e) => action();

        // 启动定时器
        timer.Start();

        // 停止并释放定时器
        timer.Stop();
        timer.Dispose();
    }
}