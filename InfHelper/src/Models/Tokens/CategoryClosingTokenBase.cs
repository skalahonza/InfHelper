using System.Linq;

namespace InfHelper.Models.Tokens
{
    public class CategoryClosingTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = { ']' };
        
        public override TokenType Type { get; } = TokenType.CategoryClosing;
    }
}