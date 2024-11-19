namespace BetterGenshinImpact.Assistant.Constant;

public class CodedEnum : Localizable
{
    public int Code { get; set; }
    public string ResourceId { get; set; }

    public CodedEnum(int code, string resourceId)
    {
        this.Code = code;
        this.ResourceId = resourceId;
    }

    // ToString 方法
    public override string ToString()
    {
        return $"Code: {Code}, RousourceId: {ResourceId}";
    }
}