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
        public string Name { get; }
        public string Hint { get; }

        private Token() { }
        public Token(short gold, short fame, ScienceSymbol symbol, TokenEffect effect, string name, string hint)
        {
            Gold = gold;
            Fame = fame;
            Symbol = symbol;
            Effect = effect;
            Name = name;
            Hint = hint;
        }

        public void GetProfit(Player player)
        {
            player.Fame += Fame;
            player.Resource.Gold += Gold;
            player.Tokens.Add(this);
            if (Symbol != ScienceSymbol.None)
            {
                player.Symbols[(int)Symbol] = true;
                int calcSymb = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (player.Symbols[i]) calcSymb++;
                }
                if (calcSymb >= 6) Game.End(Game.GameEnding.Science);
            }
            if (Effect != TokenEffect.None)
            {
                player.TokenEffects[Effect] = true;
            }
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
