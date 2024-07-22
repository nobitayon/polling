namespace Delta.Polling.Both.Common.Extensions;

public static class DecimalExtensions
{
    public static string ToMoneyDisplayText(this decimal value)
    {
        return value.ToString(FormatFor.Money);
    }
}
