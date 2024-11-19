namespace BetterGenshinImpact.Assistant.Bean;

using BetterGenshinImpact.Assistant.Constant;
using System.Collections.Generic;

public class KeyMouseOp : CodedEnum
{
    public static readonly KeyMouseOp Unkwown = new(0, "Unknown");
    public static readonly KeyMouseOp Move = new(1, "Move");
    public static readonly KeyMouseOp MoveTo = new(2, "MoveTo");
    public static readonly KeyMouseOp Click = new(3, "Click");
    public static readonly KeyMouseOp DoubleClick = new(4, "DoubleClick");
    public static readonly KeyMouseOp Drag = new(5, "Drag");
    public static readonly KeyMouseOp Press = new(6, "Press");
    public static readonly KeyMouseOp Release = new(7, "Release");
    public static readonly KeyMouseOp Input = new(8, "Input");
    public static readonly Dictionary<int, KeyMouseOp> Values = new()
    {
        { Unkwown.Code, Unkwown },
        { Move.Code,  Move },
        { MoveTo.Code,  MoveTo },
        { Click.Code, Click },
        { DoubleClick.Code, DoubleClick },
        { Drag.Code, Drag },
        { Press.Code, Press },
        { Release.Code, Release },
        { Input.Code, Input },
    };

    private KeyMouseOp(int code, string resourceId) : base(code, resourceId) {}
}
