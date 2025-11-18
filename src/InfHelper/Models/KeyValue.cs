namespace InfHelper.Models;

public class KeyValue
{
    /// <summary>
    /// Static or dynamic value. Static values are strings, dynamic values are key names wrapped inside % tags and are identifiers of other keys
    /// </summary>
    public virtual required string? Value { get; set; }

    public virtual string? PrimitiveValue => Value;
    public virtual bool IsDynamic => Value != null && Value.StartsWith('%') && Value.EndsWith('%');
    public virtual string? DynamicKeyId => IsDynamic && Value?.Length > 0 ? Value[1..^1] : null;
}

public class PureValue : KeyValue
{
    public override string PrimitiveValue => $"\"{Value}\"";
}