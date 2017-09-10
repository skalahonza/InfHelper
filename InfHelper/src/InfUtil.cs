using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using InfHelper.Models;
using InfHelper.Models.Attributes;
using InfHelper.Parsers;

namespace InfHelper
{
    public class InfUtil
    {
        public InfData Parse(string data)
        {
            var infData = new InfData();
            var parser = new ContentParser();
            parser.CategoryDiscovered += (sender, category) => infData.Categories.Add(category);
            parser.Parse(data);
            return infData;
        }

        public InfData ParseFile(string path)
        {
            var content = File.ReadAllText(path);
            return Parse(content);
        }

        public T SerializeInto<T>(string data, out InfData outputData) where T : new()
        {
            bool dereference = false;
            var o = new T();
            var t = o.GetType();
            var infData = new InfData();
            var parser = new ContentParser();

            var dict = new Dictionary<Key, PropertyInfo>();

            parser.CategoryDiscovered += (sender, category) =>
            {
                infData.Categories.Add(category);
                foreach (var property in t.GetProperties())
                {
                    if (Attribute.IsDefined(property, typeof(InfKeyValue)))
                    {
                        var attribute = (InfKeyValue) Attribute.GetCustomAttribute(property, typeof(InfKeyValue));
                        if (category.IsNamed(attribute.CategoryId))
                        {
                            var key = category[attribute.KeyId];
                            if (key != null)
                            {
                                property.SetValue(o, key.PrimitiveValue);

                                //save dynamic values for further dereferencing
                                if (attribute.DeferenceDynamicValueKeys && key.KeyValues.Any() &&
                                    key.KeyValues.Any(x => x.IsDynamic))
                                {
                                    // save for later des.
                                    dict.Add(key, property);
                                    dereference = true;
                                }
                            }
                        }
                    }
                }

                //TODO dereference keys
                if (dereference && false)
                {
                    foreach (var item in dict.ToList())
                    {
                        string value = GetPrimitiveValueForKey(category, item.Key);
                        if (value != null)
                        {
                            item.Value.SetValue(o, value);
                            dict.Remove(item.Key);
                        }                        
                    }                   
                }
            };
            parser.Parse(data);
            outputData = infData;

            //dereference keys - if some left after category dereferencing
            if (dereference)
            {
                foreach (var item in dict)
                {
                    string value = GetPrimitiveValueForKey(infData, item.Key);
                    if (value != null)
                    {
                        item.Value.SetValue(o, value);
                    }                    
                }
            }

            return o;
        }

        private string GetPrimitiveValueForKey(Category data, Key key)
        {
            if (key.KeyValues.Any())
            {
                var first = key.KeyValues.First();
                //dynamic
                if (first.IsDynamic)
                {
                    var result =
                        data.Keys
                            .FirstOrDefault(x => x.KeyValues.All(v => !v.IsDynamic)) // that has not a dynamic value
                            ?.KeyValues.First().Value; //return the first text value
                    return result;
                }
                //static
                return key.PrimitiveValue;
            }
            return "";
        }

        private string GetPrimitiveValueForKey(InfData data, Key key)
        {
            if (key.KeyValues.Any())
            {
                var first = key.KeyValues.First();
                //dynamic
                if (first.IsDynamic)
                {
                    return data.FindKeyById(first.DynamicKeyId) //find dynamic key
                        .First(x => x.KeyValues.All(v => !v.IsDynamic)) // that has not a dynamic value
                        .KeyValues.First().Value; //return the first text value
                }
                //static
                return key.PrimitiveValue;
            }
            return "";
        }

        public T SerializeFileInto<T>(string path, out InfData outputData) where T : new()
        {
            var content = File.ReadAllText(path);
            return SerializeInto<T>(content, out outputData);
        }
    }
}
