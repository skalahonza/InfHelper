using System;
using System.IO;
using System.Linq;
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
            var o = new T();
            var t = o.GetType();
            var infData = new InfData();
            var parser = new ContentParser();
            parser.CategoryDiscovered += (sender, category) =>
            {
                infData.Categories.Add(category);
                foreach (var property in t.GetProperties())
                {
                    if(Attribute.IsDefined(property, typeof(InfKeyValue)))
                    {
                        var attribute = (InfKeyValue)Attribute.GetCustomAttribute(property, typeof(InfKeyValue));
                        if (category.Name == attribute.CategoryId)
                        {
                            property.SetValue(o,category[attribute.KeyId].PrimitiveValue);
                        }
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
