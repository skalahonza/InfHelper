using System.Collections.Generic;

namespace InfHelper.Models
{
    public class Key
    {
        /// <summary>
        /// Id of the key, can be mepty (anonymous keys) 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Values spearated by comma, can be inside quotes e.g. "value"
        /// </summary>
        public List<KeyValue> KeyValues { get; set; } = new List<KeyValue>();
    }
}