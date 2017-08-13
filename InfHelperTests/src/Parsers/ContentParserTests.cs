using System.Collections.Generic;
using System.Linq;
using InfHelper.Models;
using InfHelper.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfHelperTests.Parsers
{
    [TestClass()]
    public class ContentParserTests
    {
        [TestMethod()]
        public void CategoryParsing()
        {
            string test = "[CATEGORY]";
            var categories = new List<Category>();
            var parser = new ContentParser();
            parser.CategoryDiscovered += (sender, category) => categories.Add(category);

            parser.Parse(test);

            Assert.IsTrue(categories.Count == 1 && categories.First().Name == "CATEGORY");
        }

        [TestMethod()]
        public void MultipleCategoryParsing()
        {
            string test = "[CATEGORY] \n [CATEGORY2] \n [CATEGORY3]";
            var categories = new List<Category>();
            var parser = new ContentParser();
            parser.CategoryDiscovered += (sender, category) => categories.Add(category);

            parser.Parse(test);

            Assert.IsTrue(categories.Count == 3);
            Assert.IsTrue(categories.Any(x => x.Name == "CATEGORY"));
            Assert.IsTrue(categories.Any(x => x.Name == "CATEGORY2"));
            Assert.IsTrue(categories.Any(x => x.Name == "CATEGORY3"));
        }
    }
}