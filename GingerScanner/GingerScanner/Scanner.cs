using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerScanner
{
    public class Scanner : System.IO.StringReader
    {
        public Scanner(string source) : base(source)
        {

        }

        public Token next()
        {
            List<char> result = new List<char>();
            while (true)
            {
                int next = this.Read();
                if (next == -1)
                {
                    break;
                }
                else
                {
                    char c = Convert.ToChar(next);
                    if (Char.IsWhiteSpace(c))
                    {
                        break;
                    }
                    else
                    {
                        result.Add(c);
                    }
                }
            }

            return Token.toToken(result.ToArray());
        }
    }
}
