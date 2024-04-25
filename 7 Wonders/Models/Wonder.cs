using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public class Wonder : Building
    {
        public short WarPoint { get; }
        public bool SecondTurn { get; }
        public ComplexResource Complex { get; }
        public WonderEffect Effect { get; }

        private Wonder() {}
        public Wonder(short warPoint, bool secondTurn, ComplexResource complex, WonderEffect effect, Resources cost, Resources reward, short fame, string name)
            : base(cost, reward, fame, name)
        {
            WarPoint = warPoint;
            SecondTurn = secondTurn;
            Complex = complex;
            Effect = effect;
        }

        public void GetProfit(Player player)
        {
            for (int i = 0; i < 6; i++)
            {
                 player.Resource[i] += Reward[i];
            }
            player.Fame += Fame;
            if (player == Game.FirstPlayer)
            {
                Game.WarPoints += WarPoint;
            }
            else
            {
                Game.WarPoints -= WarPoint;
            }

            if (Game.WarPoints >= 9 || Game.WarPoints <= -9)
            {
                Game.End(Game.GameEnding.War);
            }
            else
            {
                if (Complex != ComplexResource.None)
                {
                    player.ComplexResources[Complex]++;
                }

                if (Effect != WonderEffect.None)
                {
                    switch (Effect)
                    {
                        case WonderEffect.StealGold:
                        {
                            if (player.Opponent.Resource.Gold >= 3) player.Opponent.Resource.Gold -= 3;
                            else player.Opponent.Resource.Gold = 0;
                            break;
                        }
                        case WonderEffect.StealGray:
                        {
                            if (player.Opponent.GrayCards.Count > 0)
                            {
                                player.ChosenCard.TakeProfit(player.Opponent);
                                player.Opponent.GrayCards.Remove((GrayCard)player.ChosenCard);
                                Game.DiscardedCards.Add(player.ChosenCard);
                            }
                            break;
                        }
                        case WonderEffect.StealBrown:
                        {
                            if (player.Opponent.BrownCards.Count > 0)
                            {
                                player.ChosenCard.TakeProfit(player.Opponent);
                                player.Opponent.BrownCards.Remove((BrownCard)player.ChosenCard);
                                Game.DiscardedCards.Add(player.ChosenCard);
                            }
                            break;
                        }
                        case WonderEffect.TakeDiscard:
                        {
                            if (Game.DiscardedCards.Count > 0)
                            {
                                player.ChosenCard.GetProfit(player);
                            }
                            break;
                        }
                        case WonderEffect.Token:
                        {
                            player.ChosenToken.GetProfit(player);
                            break;
                        }
                    }
                }
            }
        }

        public enum WonderEffect
        {
            None,
            StealGold,
            StealGray,
            StealBrown,
            TakeDiscard,
            Token
        }
    }
}
