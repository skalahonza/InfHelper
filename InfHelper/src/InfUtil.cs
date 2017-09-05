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

            var dict = new Dictionary<string, PropertyInfo>();

            parser.CategoryDiscovered += (sender, category) =>
            {
                infData.Categories.Add(category);
                foreach (var property in t.GetProperties())
                {
                    if (Attribute.IsDefined(property, typeof(InfKeyValue)))
                    {
                        var attribute = (InfKeyValue)Attribute.GetCustomAttribute(property, typeof(InfKeyValue));
                        if (category.Name == attribute.CategoryId)
                        {
                            var key = category[attribute.KeyId];
                            property.SetValue(o, key.PrimitiveValue);

                            //save dynamic values for further dereferencing
                            if (attribute.DeferenceDynamicValueKeys && key.KeyValues.Count == 1 && key.KeyValues.First().IsDynamic)
                            {
                                // save for later des.
                                dict.Add(key.KeyValues.First().DynamicKeyId, property);
                                dereference = true;
                            }                            
                        }
                    }
                }

                //des what left
                if (dereference)
                {
                    foreach (var categoryKey in category.Keys)
                    {
                        if (dict.Keys.Any())
                            if (categoryKey.Id != null && dict.ContainsKey(categoryKey.Id))
                                dict[categoryKey.Id].SetValue(o, categoryKey.PrimitiveValue);
                    }
                }
            };
            parser.Parse(data);

            outputData = infData;
            return o;
        }

        public T SerializeFileInto<T>(string path, out InfData outputData) where T : new()
        {
            var content = File.ReadAllText(path);
            return SerializeInto<T>(content, out outputData);
        }
    }
}
