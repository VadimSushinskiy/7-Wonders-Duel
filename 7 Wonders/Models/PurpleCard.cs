using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class PurpleCard : Card
    {
        public Guild GuildEffect { get; }

        private PurpleCard() { }

        public PurpleCard(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name, Guild guild) 
            : base(epochNumber, linkToBuy, addedLink, cost, reward, fame, name)
        {
            GuildEffect = guild;
        }

        public enum Guild
        {
            Wonder,
            Gold,
            Green,
            Resources,
            Yellow,
            Blue,
            Red
        }

        public override void GetProfit(Player player)
        {
            base.GetProfit(player);
            player.Guilds[GuildEffect] = true;
            switch (GuildEffect)
            {
                case Guild.Green:
                {
                    player.Resource.Gold += (short)Math.Max(player.GreenCards.Count, player.Opponent.GreenCards.Count);
                    break;
                }
                case Guild.Red:
                {
                    player.Resource.Gold += (short)Math.Max(player.RedCards.Count, player.Opponent.RedCards.Count);
                    break;
                }
                case Guild.Yellow:
                {
                    player.Resource.Gold += (short)Math.Max(player.YellowCards.Count, player.Opponent.YellowCards.Count);
                    break;
                }
                case Guild.Blue:
                {
                    player.Resource.Gold += (short)Math.Max(player.BlueCards.Count, player.Opponent.BlueCards.Count);
                    break;
                }
                case Guild.Resources:
                {
                    player.Resource.Gold += (short)Math.Max(player.GrayCards.Count + player.BrownCards.Count, player.Opponent.GrayCards.Count + player.Opponent.BrownCards.Count);
                    break;
                }
            }
            player.PurpleCards.Add(this);
        }
    }
}
