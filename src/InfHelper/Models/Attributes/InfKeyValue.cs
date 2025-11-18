using System;

namespace InfHelper.Models.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class InfKeyValue(string categoryId, string keyId, bool deferenceDynamicValueKeys = false) : Attribute
{
    public readonly string CategoryId = categoryId;
    public readonly string KeyId = keyId;
    public readonly bool DeferenceDynamicValueKeys = deferenceDynamicValueKeys;
}