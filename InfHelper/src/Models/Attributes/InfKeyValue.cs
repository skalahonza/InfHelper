using System;

namespace InfHelper.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class InfKeyValue : Attribute
    {
        public readonly string CategoryId;
        public readonly string KeyId;
        public readonly bool DeferenceDynamicValueKeys;

        public InfKeyValue(string categoryId, string keyId, bool deferenceDynamicValueKeys = false)
        {
            CategoryId = categoryId;
            KeyId = keyId;
            DeferenceDynamicValueKeys = deferenceDynamicValueKeys;
        }
    }
}