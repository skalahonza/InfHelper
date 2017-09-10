namespace InfHelper.Models
{
    public class KeyValue
    {
        /// <summary>
        /// Static or dynamic value. Static values are strings, dynamic values are key names wrapped inside % tags and are identifiers of other keys
        /// </summary>
        public virtual string Value { get; set; }

        public virtual string PrimitiveValue => Value;
        public virtual bool IsDynamic => Value != null && Value.StartsWith("%") && Value.EndsWith("%");
        public virtual string DynamicKeyId => IsDynamic && Value.Length > 0 ? Value.Substring(1, Value.Length - 2) : null;
    }

    public class PureValue : KeyValue
    {
        public override string PrimitiveValue => $"\"{Value}\"";
    }
}