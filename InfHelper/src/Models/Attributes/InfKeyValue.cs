using System;

namespace InfHelper.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class InfKeyValue : Attribute
    {
        private readonly string categoryId;
        private readonly string keyId;

        public InfKeyValue(string CategoryId, string KeyId)
        {
            categoryId = CategoryId;
            keyId = KeyId;
        }
    }
}