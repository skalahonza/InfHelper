using System.Collections.Generic;

namespace InfHelper.Models.Tokens
{
    public class LetterToken : IToken
    {
        public char[] Symbols { get; }
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.Letter;

        public LetterToken()
        {
            var symbols = new List<char> { '.', '_', '\\', '%' };

            //add a-z
            for (char i = 'a'; i <= 'z'; i++)
                symbols.Add(i);

            //add A-Z
            for (char i = 'A'; i <= 'Z'; i++)
                symbols.Add(i);

            // 0-9
            for (char i = '0'; i <= '9'; i++)
                symbols.Add(i);

            Symbols = symbols.ToArray();
        }
    }
}