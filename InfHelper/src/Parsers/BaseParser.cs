using System.Threading.Tasks;

namespace InfHelper.Parsers
{
    public abstract class BaseParser<T>
    {

        public abstract Task<T> ParseToken(string expression, int index);
    }
}