namespace Delta.Polling.FrontEnd.Logics.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SpecialValueAttribute(SpecialValueType valueType) : Attribute
{
    public SpecialValueType ValueType { get; } = valueType;
}

public enum SpecialValueType
{
    Json = 0,
    Xml = 1
}
