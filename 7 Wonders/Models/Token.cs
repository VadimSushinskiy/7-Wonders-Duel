using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class Token
    {
        public short Gold { get; }
        public short Fame { get; }
        public ScienceSymbol Symbol { get; }
        public TokenEffect Effect { get; }

        private Token() { }
        public Token(short gold, short fame, ScienceSymbol symbol, TokenEffect effect)
        {
            Gold = gold;
            Fame = fame;
            Symbol = symbol;
            Effect = effect;
        }

        public enum TokenEffect
        {
            None,
            Economy,
            Math,
            Strategy,
            Theology,
            Urbanism
        }
    }
}
