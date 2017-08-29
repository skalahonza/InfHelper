using System.IO;
using InfHelper.Models;
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
    }
}
