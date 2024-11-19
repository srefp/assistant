using System;

namespace BetterGenshinImpact.Assistant.Engine.Operation;

public class RouteKeys
{
    public static readonly string ID = "id";
    public static readonly string NAME = "name";
    public static readonly string COMMENT = "comment";
    public static readonly string DELAY_BOSS = "delayBoss";
    public static readonly string DELAY_TRACK = "delayTrack";
    public static readonly string DELAY_MAP = "delayMap";
    public static readonly string DELAY_TP = "delayTp";

    /**** F2 传送 ****/
    public static readonly string BOSS = "boss";
    public static readonly string POS = "pos";
    public static readonly string NARROW = "narrow";
    public static readonly string SELECT = "select";
    public static readonly string FLOWER = "flower";
    public static readonly string DRAG = "drag";
    public static readonly string WHEEL = "wheel";

    /**** 直接传送 ****/
    public static readonly string POS_D = "posD";
    public static readonly string NARROW_D = "narrowD";
    public static readonly string SELECT_D = "selectD";
    public static readonly string FLOWER_D = "flowerD";
    public static readonly string DRAG_D = "dragD";
    public static readonly string WHEEL_D = "wheelD";

    /**** 选地区传送 ****/
    public static readonly string AREA = "area";
    public static readonly string POS_A = "posA";
    public static readonly string NARROW_A = "narrowA";
    public static readonly string SELECT_A = "selectA";
    public static readonly string FLOWER_A = "flowerA";
    public static readonly string DRAG_A = "dragA";
    public static readonly string WHEEL_A = "wheelA";

    /**** 已经弃用的属性 ****/
    [Obsolete("This property is deprecated. Use BOSS instead.")]
    public static readonly string MONSTER = "monster";
}
