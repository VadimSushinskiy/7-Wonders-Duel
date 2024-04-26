using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class RedCard : Card
    {
        public short WarPoint { get; private set; }

        private RedCard() {}
        public RedCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, short warPoint)
        : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            WarPoint = (warPoint >= 0) ? warPoint : (short)0;
        }
        public override void GetProfit(Player player)
        {
            base.GetProfit(player);
            if (player == Game.FirstPlayer)
            {
                Game.WarPoints += WarPoint;
                if (player.TokenEffects[Token.TokenEffect.Strategy]) Game.WarPoints++;
            }
            else
            {
                Game.WarPoints -= WarPoint;
                if (player.TokenEffects[Token.TokenEffect.Strategy]) Game.WarPoints--;
            }
            Game.WarPunish();
            player.RedCards.Add(this);

            if (Game.WarPoints >= 10 || Game.WarPoints <= -10)
            {
                Game.End(Game.GameEnding.War);
            }
        }
    }
}
