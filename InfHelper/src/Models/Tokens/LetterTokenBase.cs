using System;
using System.Collections.Generic;
using System.Linq;

namespace InfHelper.Models.Tokens
{
    public class LetterTokenBase : TokenBase
    {
        public override char[] Symbols
        {
            get
            {
                var symbols = new HashSet<char>();
                for (int i = char.MinValue; i <= char.MaxValue; i++)
                {
                    char c = Convert.ToChar(i);
                    if (!char.IsControl(c) && !char.IsWhiteSpace(c))
                        symbols.Add(c);
                }
                return symbols.ToArray();
            }
        }

        public override TokenType Type { get; } = TokenType.Letter;

        public override bool IsToken(char c)
        {
            return (!char.IsControl(c) && !char.IsWhiteSpace(c));
        }
    }
}