using System.Linq;

namespace InfHelper.Models.Tokens
{
    public abstract class TokenBase
    {
        public abstract char[] Symbols { get; }
        public virtual bool IsToken(char c)
        {
            return Symbols.Contains(c);
        }
        public char Symbol { get; set; }
        public abstract TokenType Type { get; }
    }
}