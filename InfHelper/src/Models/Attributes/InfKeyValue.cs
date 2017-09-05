using System;

namespace InfHelper.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class InfKeyValue : Attribute
    {
        public readonly string CategoryId;
        public readonly string KeyId;

        public InfKeyValue(string categoryId, string keyId)
        {
            CategoryId = categoryId;
            KeyId = keyId;
        }
    }
}