using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BetterGenshinImpact.Assistant.Bean;
using BetterGenshinImpact.Assistant.Engine.Operation;

namespace BetterGenshinImpact.Assistant.Engine.Parser;

public class RouteUtil
{
    public static List<TpPoint> ParseFile(string filePath)
    {
        List<TpPoint> res = [];
        // 读取文件
        string[] lines = File.ReadAllLines(filePath);
        foreach (string lineItem in lines)
        {
            string line = lineItem.Trim();
            if (lineItem.Contains("--"))
            {
                line = lineItem[..lineItem.IndexOf("--")].Trim();
            }
            if (lineItem.Contains(';'))
            {
                line = lineItem[..lineItem.IndexOf(';')].Trim();
            }

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string pattern = @",? *(\w+): *";

            // 根据匹配到的keys进行分割
            string[] values = Regex.Split(line, pattern);
            List<string> keyValues = [.. values.Where(s => !string.IsNullOrEmpty(s))];
            int len = keyValues.Count;
            var tpPoint = new TpPoint();
            for (int index = 0; index < len; index += 2)
            {
                string key = keyValues[index];
                string value = keyValues[index + 1];
                if (key == RouteKeys.ID)
                {
                    tpPoint.Id = StringToString(value);
                }
                if (key == RouteKeys.BOSS)
                {
                    tpPoint.Boss = StringToIntList(value);
                }
                if (key == RouteKeys.MONSTER)
                {
                    tpPoint.Boss = StringToIntList(value);
                }
                else if (key == RouteKeys.DELAY_BOSS)
                {
                    tpPoint.DelayBook = StringToInt(value);
                }
                else if (key == RouteKeys.DELAY_TRACK)
                {
                    tpPoint.DelayTrack = StringToInt(value);
                }
                else if (key == RouteKeys.DELAY_MAP)
                {
                    tpPoint.DelayMap = StringToInt(value);
                }
                else if (key == RouteKeys.DELAY_TP)
                {
                    tpPoint.DelayTp = StringToInt(value);
                }

                // F2传送
                else if (key == RouteKeys.POS)
                {
                    tpPoint.Pos = StringToIntList(value);
                }
                else if (key == RouteKeys.NARROW)
                {
                    tpPoint.Narrow = StringToInt(value);
                }
                else if (key == RouteKeys.SELECT)
                {
                    tpPoint.Select = StringToInt(value);
                }
                else if (key == RouteKeys.FLOWER)
                {
                    tpPoint.Flower = StringToBool(value);
                }
                else if (key == RouteKeys.DRAG)
                {
                    tpPoint.Drag = StringToIntList(value);
                }

                // 直接传送
                else if (key == RouteKeys.POS_D)
                {
                    tpPoint.PosD = StringToIntList(value);
                }
                else if (key == RouteKeys.NARROW_D)
                {
                    tpPoint.NarrowD = StringToInt(value);
                }
                else if (key == RouteKeys.SELECT_D)
                {
                    tpPoint.SelectD = StringToInt(value);
                }
                else if (key == RouteKeys.FLOWER_D)
                {
                    tpPoint.FlowerD = StringToBool(value);
                }
                else if (key == RouteKeys.DRAG_D)
                {
                    tpPoint.DragD = StringToIntList(value);
                }

                // 选地区传送
                else if (key == RouteKeys.AREA)
                {
                    tpPoint.Area = StringToInt(value);
                }
                else if (key == RouteKeys.POS_A)
                {
                    tpPoint.PosA = StringToIntList(value);
                }
                else if (key == RouteKeys.NARROW_A)
                {
                    tpPoint.NarrowA = StringToInt(value);
                }
                else if (key == RouteKeys.SELECT_A)
                {
                    tpPoint.SelectA = StringToInt(value);
                }
                else if (key == RouteKeys.FLOWER_A)
                {
                    tpPoint.FlowerA = StringToBool(value);
                }
                else if (key == RouteKeys.DRAG_A)
                {
                    tpPoint.DragA = StringToIntList(value);
                }

                else if (key == RouteKeys.NAME)
                {
                    tpPoint.Name = StringToString(value);
                }
                else if (key == RouteKeys.COMMENT)
                {
                    tpPoint.Comment = StringToString(value);
                }
            }
            res.Add(tpPoint);
        }
        return res;
    }

    private static bool StringToBool(string str)
    {
        return bool.Parse(str.Trim());
    }

    private static string StringToString(string str)
    {
        return str.Replace("\"", "").Trim();
    }

    private static int StringToInt(string str)
    {
        return int.Parse(str.Trim());
    }

    private static List<int> StringToIntList(string str)
    {
        List<int> res = [];
        if (str.StartsWith('['))
        {
            str = str.Replace("[", "").Replace("]", "");
            string[] tmpValues = Regex.Split(str, @", *");
            string[] numbers = [.. tmpValues.Where(s => !string.IsNullOrEmpty(s))];
            foreach (string number in numbers)
            {
                res.Add(int.Parse(number.Trim()));
            }
        }
        return res;
    }
}
