using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public abstract class Card : Building
    {
        public byte EpochNumber { get; protected set; } = 1;
        public BuildLink LinkToBuy { get; protected set; }
        public BuildLink AddedLink { get; protected set; }
        public bool IsAvailable { get; set; } 

        protected Card() {}
        protected Card(byte epochNumber, BuildLink linkToBuy, BuildLink addedLink, Resources cost, Resources reward, short fame, string name)
        : base(cost, reward, fame, name)
        {
            EpochNumber = epochNumber;
            LinkToBuy = linkToBuy;
            AddedLink = addedLink;
        }
        protected Card(byte epochNumber)
        {
            EpochNumber = epochNumber;
        }

        public virtual void GetProfit(Player player)
        {
            for (int i = 0; i < 6; i++)
            {
                player.Resource[i] += Reward[i];
            }
            player.Fame += Fame;
            if (AddedLink != BuildLink.None)
            {
                player.BuildLinks[AddedLink] = true;
            }
            if (LinkToBuy != BuildLink.None && player.BuildLinks[LinkToBuy] && player.TokenEffects[Token.TokenEffect.Urbanism])
            {
                player.Resource.Gold += 4;
            } 
        }

        public void TakeProfit(Player player)
        {
            for (int i = 0; i < 6; i++)
            {
                player.Resource[i] -= Reward[i];
            }
            player.Fame -= Fame;
            if (AddedLink != BuildLink.None)
            {
                player.BuildLinks[AddedLink] = false;
            }
        }
    }
}
