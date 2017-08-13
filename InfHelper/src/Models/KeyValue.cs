using System.Collections.Generic;

namespace InfHelper.Models
{
    public class KeyValue
    {
        /// <summary>
        /// Static or dynamic value. Static values are strings, dynamic values are key names wrapped inside % tags and are identifiers of other keys
        /// </summary>
        public string Value { get; set; }
        public bool IsDynamic => Value.StartsWith("%") && Value.EndsWith("%");
        public string DynamicKeyId => IsDynamic && Value.Length > 0 ? Value.Substring(1, Value.Length - 2) : null;
    }
}