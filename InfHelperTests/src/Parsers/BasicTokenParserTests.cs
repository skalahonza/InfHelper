using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfHelper.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers.Tests
{
    [TestClass()]
    public class BasicTokenParserTests
    {
        [TestMethod()]
        public void BasicTokenParserTest()
        {
            string formula = "[TEST]";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>()
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>());
            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.ParseToken(formula);

            Assert.AreEqual(formula, result);
        }
    }
}