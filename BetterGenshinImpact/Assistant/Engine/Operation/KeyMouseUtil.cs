using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using BetterGenshinImpact.Core.Simulator;
using Microsoft.Extensions.Logging;
using Vanara.PInvoke;

namespace BetterGenshinImpact.Assistant.Engine.Operation;

class KeyMouseUtil
{
    private static readonly int _SCREEN_WIDTH = Screen.PrimaryScreen.Bounds.Width;
    private static readonly int _SCREEN_HEIGHT = Screen.PrimaryScreen.Bounds.Height;
    private static readonly int _FACTOR = 65535;
    private static readonly ILogger _logger = App.GetLogger<KeyMouseUtil>();

    public static void LongClick(int[] point)
    {
        var res = PhysicalPos(point);
        Simulation.MouseEvent.Move(res[0], res[1]);
        Simulation.SendInput.Mouse.LeftButtonDown();
        Sleep(60);
        Simulation.SendInput.Mouse.LeftButtonUp();
    }

    public static void Click(int[] point)
    {
        var res = PhysicalPos(point);
        _logger.LogError($"点击：{res[0]}, {res[1]}");
        Simulation.MouseEvent.Move(res[0], res[1]);
        Simulation.SendInput.Mouse.LeftButtonClick();
    }
    
    public static void ClickRight()
    {
        // TODO: 实现右键点击
    }

    public static int[] GetMousePos()
    {
        POINT point;
        User32.GetCursorPos(out point);
        return [point.x, point.y];
    }

    public static int[] LogicalPos()
    {
        var point = GetMousePos();
        return
        [
            point[0] * _FACTOR / (_SCREEN_WIDTH - 1),
            point[1] * _FACTOR / (_SCREEN_HEIGHT - 1)
        ];
    }

    public static int[] PhysicalPos(int[] lPos)
    {
        return
        [
            lPos[0] * (_SCREEN_WIDTH - 1) / _FACTOR,
            lPos[1] * (_SCREEN_HEIGHT - 1) / _FACTOR
        ];
    }

    public static void Send(User32.VK key)
    {
        Simulation.SendInput.Keyboard.KeyPress(key, 0, 0, 0);
    }

    public static void Sleep(int milliSeconds)
    {
        Thread.Sleep(milliSeconds);
    }

    public static void Wheel(int rowWheelNum)
    {
        _logger.LogError($"滚动了{rowWheelNum}");
        if (rowWheelNum > 0)
        {
            for (var i = 0; i < rowWheelNum; i++)
            {
                Simulation.SendInput.Mouse.VerticalScroll(-1);
                Sleep(10);
            }
        }
        else
        {
            for (var i = 0; i < -rowWheelNum; i++)
            {
                Simulation.SendInput.Mouse.VerticalScroll(1);
                Sleep(10);
            }
        }
    }
    
    public static void Drag(int[] drag)
    {
        var start = PhysicalPos([drag[0], drag[1]]);
        var end = PhysicalPos([drag[2], drag[3]]);
        
        Simulation.SendInput.Mouse.MoveMouseTo(start[0], start[1]);
        Simulation.SendInput.Mouse.LeftButtonDown();
        Simulation.SendInput.Mouse.MoveMouseTo(end[0], end[1]);
        Simulation.SendInput.Mouse.LeftButtonUp();
    }
}