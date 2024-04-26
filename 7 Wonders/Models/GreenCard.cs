using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class GreenCard : Card
    {
        public ScienceSymbol Symbol { get; private set; }

        private GreenCard() { }
        public GreenCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, ScienceSymbol symbol)
        : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            Symbol = symbol;
        }

        public override void GetProfit(Player player)
        {
            base.GetProfit(player);
            if (!player.Symbols[(int)Symbol])
            {
                player.Symbols[(int)Symbol] = true;
            }
            else
            {
                int tokenCount = 5;
                foreach (var token in Game.GameTokens)
                {
                    if (player.Tokens.Contains(token) || player.Opponent.Tokens.Contains(token)) 
                    {
                        tokenCount--;
                    }
                }
                if (tokenCount > 0)
                {
                    player.ChosenToken.GetProfit(player);
                }
            }
            player.GreenCards.Add(this);
            int calcSymb = 0;
            for (int i = 0; i < 7; i++)
            {
                if (player.Symbols[i]) calcSymb++;
            }
            if (calcSymb >= 6) Game.End(Game.GameEnding.Science);
        }
    }
}
