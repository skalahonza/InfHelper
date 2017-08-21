using InfHelper.Models;
using InfHelper.Parsers;

namespace InfHelper
{
    public class InfUtility
    {
        public InfData Parse(string expression)
        {
            var parser = new ContentParser();
            var data = new InfData();
            parser.CategoryDiscovered += (sender, category) => data.Categories.Add(category);

            return data;
        }
    }
}
