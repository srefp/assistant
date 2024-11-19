using BetterGenshinImpact.Assistant.Constant;
using System.Collections.Generic;

namespace BetterGenshinImpact.Assistant.Bean;

public class TpMethod : CodedEnum
{
    public static readonly TpMethod Unknown = new(0, "Unkown");
    public static readonly TpMethod F2 = new(1, "F2");
    public static readonly TpMethod Area = new(2, "Area");
    public static readonly TpMethod Direct = new(3, "Direct");
    public static readonly Dictionary<int, TpMethod> Values = new()
    {
        { Unknown.Code, Unknown },
        { F2.Code, F2 },
        { Area.Code, Area },
        { Direct.Code, Direct },
    };
    private TpMethod(int code, string resourceId) : base(code, resourceId) { }
}