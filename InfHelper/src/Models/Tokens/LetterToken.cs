using System;
using System.Collections.Generic;
using System.Linq;

namespace InfHelper.Models.Tokens
{
    public class LetterToken : IToken
    {
        public char[] Symbols { get; }
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.Letter;

        public LetterToken()
        {
            var symbols = new HashSet<char> { '.', '_', '\\', '%', '&' ,':','(',')','$','{','}','-','/', '\'',';','*','+'};

            //add a-z
            for (char i = 'a'; i <= 'z'; i++)
                symbols.Add(i);

            //add A-Z
            for (char i = 'A'; i <= 'Z'; i++)
                symbols.Add(i);

            // 0-9
            for (char i = '0'; i <= '9'; i++)
                symbols.Add(i);

            for (int i = char.MinValue; i <= char.MaxValue; i++)
            {
                char c = Convert.ToChar(i);
                if (!char.IsControl(c) && !char.IsWhiteSpace(c))
                    symbols.Add(c);
            }
            Symbols = symbols.ToArray();
        }
    }
}