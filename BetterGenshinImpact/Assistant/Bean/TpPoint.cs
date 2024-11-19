using System.Collections.Generic;
using System.Text;
using BetterGenshinImpact.Assistant.Engine.Operation;

namespace BetterGenshinImpact.Assistant.Bean;

// 传送点
public class TpPoint
{
    // ID
    public string? Id { get; set; }

    // 名称
    public string? Name { get; set; }

    // 备注
    public string? Comment { get; set; }

    // 选怪延迟
    public int? DelayBook { get; set; }

    // 点追踪延迟
    public int? DelayTrack { get; set; }

    // 开图延迟
    public int? DelayMap { get; set; }

    // 选锚点延迟
    public int? DelayTp { get; set; }
    // 等待点确认
    public int? DelayConfirm { get; set; }

    public List<int>? Boss { get; set; }
    // F2传送
    public List<int>? Pos { get; set; }

    public int? Narrow { get; set; }

    public int? Select { get; set; }

    // 是否有地脉花
    public bool? Flower { get; set; }

    public List<int>? Drag { get; set; }
    public int? Wheel { get; set; }

    // 直接传送
    public List<int>? PosD { get; set; }

    public int? NarrowD { get; set; }

    public int? SelectD { get; set; }

    public bool? FlowerD { get; set; }

    public List<int>? DragD { get; set; }
    public int? WheelD { get; set; }

    // 选地区传送
    public int? Area { get; set; }

    public List<int>? PosA { get; set; }

    public int? NarrowA { get; set; }

    public int? SelectA { get; set; }

    public bool? FlowerA { get; set; }

    public List<int>? DragA { get; set; }
    public int? WheelA { get; set; }

    public override string ToString()
    {
        StringBuilder builder = new();
        if (Id != null)
        {
            builder.Append($"{RouteKeys.ID}: \"{Id}\", ");
        }
        if (Boss != null)
        {
            builder.Append($"{RouteKeys.BOSS}: [{string.Join(", ", Boss)}], ");
        }

        // F2 传送
        if (Pos != null)
        {
            builder.Append($"{RouteKeys.POS}: [{string.Join(", ", Pos)}], ");
        }
        if (Narrow != null)
        {
            builder.Append($"{RouteKeys.NARROW}: {Narrow}, ");
        }
        if (Select != null)
        {
            builder.Append($"{RouteKeys.SELECT}: {Select}, ");
        }
        if (Flower != null)
        {
            var value = Flower == true ? "true" : "false";
            builder.Append($"{RouteKeys.FLOWER}: {value}, ");
        }
        if (Drag != null)
        {
            builder.Append($"{RouteKeys.DRAG}: [{string.Join(", ", Drag)}], ");
        }
        if (Wheel != null)
        {
            builder.Append($"{RouteKeys.WHEEL}: {Wheel}, ");
        }

        // 直接传送
        if (PosD != null)
        {
            builder.Append($"{RouteKeys.POS_D}: [{string.Join(", ", PosD)}], ");
        }
        if (NarrowD != null)
        {
            builder.Append($"{RouteKeys.NARROW_D}: {NarrowD}, ");
        }
        if (SelectD != null)
        {
            builder.Append($"{RouteKeys.SELECT_D}: {SelectD}, ");
        }
        if (FlowerD != null)
        {
            var value = FlowerD == true ? "true" : "false";
            builder.Append($"{RouteKeys.FLOWER_D}: {value}, ");
        }
        if (DragD != null)
        {
            builder.Append($"{RouteKeys.DRAG_D}: [{string.Join(", ", DragD)}], ");
        }
        if (WheelD != null)
        {
            builder.Append($"{RouteKeys.WHEEL_D}: {WheelD}, ");
        }

        // 选地区传送
        if (Area != null)
        {
            builder.Append($"{RouteKeys.AREA}: [{string.Join(", ", Area)}], ");
        }
        if (PosA != null)
        {
            builder.Append($"{RouteKeys.POS_A}: [{string.Join(", ", PosA)}], ");
        }
        if (NarrowA != null)
        {
            builder.Append($"{RouteKeys.NARROW_A}: {NarrowA}, ");
        }
        if (SelectA != null)
        {
            builder.Append($"{RouteKeys.SELECT_A}: {SelectA}, ");
        }
        if (FlowerA != null)
        {
            var value = FlowerA == true ? "true" : "false";
            builder.Append($"{RouteKeys.FLOWER_A}: {value}, ");
        }
        if (DragA != null)
        {
            builder.Append($"{RouteKeys.DRAG_A}: [{string.Join(", ", DragA)}], ");
        }
        if (WheelA != null)
        {
            builder.Append($"{RouteKeys.WHEEL_A}: {WheelA}, ");
        }

        // 延迟
        if (DelayBook != null)
        {
            builder.Append($"{RouteKeys.DELAY_BOSS}: {DelayBook}, ");
        }
        if (DelayTrack != null)
        {
            builder.Append($"{RouteKeys.DELAY_TRACK}: {DelayTrack}, ");
        }
        if (DelayMap != null)
        {
            builder.Append($"{RouteKeys.DELAY_MAP}: {DelayMap}, ");
        }
        if (DelayTp != null)
        {
            builder.Append($"{RouteKeys.DELAY_TP}: {DelayTp}, ");
        }

        // 名称与备注
        if (Name != null)
        {
            builder.Append($"{RouteKeys.NAME}: \"{Name}\", ");
        }
        if (Comment != null)
        {
            builder.Append($"{RouteKeys.COMMENT}: \"{Comment}\", ");
        }
        return builder.ToString()[..(builder.Length - 2)];
    }
}
