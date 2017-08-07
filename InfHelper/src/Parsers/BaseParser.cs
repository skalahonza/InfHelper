using System.Threading.Tasks;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public abstract class BaseParser<T>
    {

        public abstract Task<T> ParseToken(string expression, int index);
    }

    public class TokenParser
    {


        public IToken ParseToken(string line)
        {
            
        }
    }
}