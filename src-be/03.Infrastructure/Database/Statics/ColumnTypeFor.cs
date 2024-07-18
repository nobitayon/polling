namespace Delta.Polling.Infrastructure.Database.Statics;

public static class ColumnTypeFor
{
    public const string Money = "money";
    public const string NVarcharMax = "nvarchar(max)";

    public static string NVarchar(int length)
    {
        return $"nvarchar({length})";
    }
}
