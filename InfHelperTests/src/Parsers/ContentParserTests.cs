using System;
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
            int CATEGORIES_COUNT = new Random().Next(1,10);
            var testCategories = new List<string>();

            for (int i = 1; i <= CATEGORIES_COUNT; i++)
            {
                testCategories.Add($"[CATEGORY{i}]");
            }

            string test = string.Join(" \n ", testCategories);
            
            var categories = new List<Category>();
            var parser = new ContentParser();
            parser.CategoryDiscovered += (sender, category) => categories.Add(category);

            parser.Parse(test);

            Assert.IsTrue(categories.Count == CATEGORIES_COUNT);

            for (var i = 1; i <= CATEGORIES_COUNT; i++)
            {
                Assert.IsTrue(categories.Any(x => x.Name == $"CATEGORY{i}"));
            }
        }
    }
}