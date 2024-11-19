using System.Collections.Generic;

namespace BetterGenshinImpact.Assistant.Constant;

public class EnumUtil
{
    // 获取值
    public static T? GetByKey<T>(int code, Dictionary<int, T> values) where T : CodedEnum
    {
        foreach (var pair in values)
        {
            if (pair.Value.Code == code)
            {
                return (T)pair.Value;
            }
        }
        return default;
    }

    // 获取值
    public static T GetByKey<T>(int code, Dictionary<int, T> values, T def) where T : CodedEnum
    {
        foreach (var pair in values)
        {
            if (pair.Value.Code == code)
            {
                return (T)pair.Value;
            }
        }
        return def;
    }
}