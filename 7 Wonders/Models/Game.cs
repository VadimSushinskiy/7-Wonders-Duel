using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models
{
    public static class Game
    {
        public static Player FirstPlayer { get; private set; }
        public static Player SecondPlayer { get; private set; }
        public static Player CurrentPlayer { get; set; }
        public static int WarPoints { get; set; }
        public static byte Epoch { get; set; }
        public static int CardNumber { get; set; }
        public static List<Card> FirstEpochCards { get; set; }
        public static List<Card> SecondEpochCards { get; set; }
        public static List<Card> ThirdEpochCards { get; set; }
        public static List<Card> DiscardedCards { get; set; }
        public static Dictionary<Card, List<Card>> FirstEpochGraph { get; set; }
        public static Dictionary<Card, List<Card>> SecondEpochGraph { get; set; }
        public static Dictionary<Card, List<Card>> ThirdEpochGraph { get; set; }
        public static List<Token> Tokens { get; set; }
        public static bool WonderIsAvailable { get; set; }
        public static Random rnd = new Random();

        public static void InitializeGame()
        {
            FirstPlayer = new Player("Player 1");
            SecondPlayer = new Player("Player 2");
            CurrentPlayer = FirstPlayer;
            FirstPlayer.Opponent = SecondPlayer;
            SecondPlayer.Opponent = FirstPlayer;
            WarPoints = 0;
            Epoch = 1;
            CardNumber = 20;
            FirstEpochCards = new List<Card>()
            {
                new BlueCard(1, BuildLink.None, BuildLink.Moon, new Resources(), new Resources(), 3, "Altar"),
                new GreenCard(1, BuildLink.None, BuildLink.Gear, new Resources() { Gold = 2 }, new Resources(), 0, "Apothecary", ScienceSymbol.Pounder),
                new BlueCard(1, BuildLink.None, BuildLink.Water, new Resources() {Stone = 1}, new Resources(), 3, "Baths"),
                new GrayCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Paper = 1}, 0, "BookPublishing"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Brick = 1}, 0, "ClayMine"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Brick = 1}, 0, "ClayQuarry"),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "ClaySupply", Sale.BrickSale),
                new RedCard(1, BuildLink.None, BuildLink.Sword, new Resources() {Brick = 1}, new Resources(), 0, "Garrison", 1),
                new GrayCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Glass = 1}, 0, "GlassWorkshop"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Stone = 1}, 0, "GraniteQuarry"),
                new GreenCard(1, BuildLink.None, BuildLink.None, new Resources() {Glass = 1}, new Resources(), 1, "Herbalist", ScienceSymbol.Wheel),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 1}, new Resources() {Wood = 1}, 0, "LumberCamp"),
                new RedCard(1, BuildLink.None, BuildLink.Tower, new Resources() {Gold = 2}, new Resources(), 0, "Palisade", 1),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Stone = 1}, 0, "Quarry"),
                new GreenCard(1, BuildLink.None, BuildLink.Book, new Resources() {Gold = 2}, new Resources(), 0, "Scriptorium", ScienceSymbol.Feather),
                new RedCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources(), 0, "SecurityTower", 1),
                new RedCard(1, BuildLink.None, BuildLink.Horseshoe, new Resources() {Wood = 1}, new Resources(), 0, "Stable", 1),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "StoneSupply", Sale.StoneSale),
                new YellowCard(1, BuildLink.None, BuildLink.Jug, new Resources(), new Resources() {Gold = 4}, 0, "Tavern", Sale.None),
                new BlueCard(1, BuildLink.None, BuildLink.Mask, new Resources(), new Resources(), 3, "Theater"),
                new BrownCard(1, BuildLink.None, BuildLink.None, new Resources(), new Resources() {Wood = 1}, 0, "TimberFelling"),
                new YellowCard(1, BuildLink.None, BuildLink.None, new Resources() {Gold = 3}, new Resources(), 0, "WoodSupply", Sale.WoodSale),
                new GreenCard(1, BuildLink.None, BuildLink.None, new Resources() {Paper = 1}, new Resources(), 1, "Workshop", ScienceSymbol.Ruler)
            };
            SecondEpochCards = new List<Card>();
            ThirdEpochCards = new List<Card>();
            DiscardedCards = new List<Card>();
            Tokens = new List<Token>();
            WonderIsAvailable = true;
            Shuffle(FirstEpochCards);
            FirstEpochCards.RemoveAt(22);
            FirstEpochCards.RemoveAt(21);
            FirstEpochCards.RemoveAt(20);
            FirstEpochGraph = new Dictionary<Card, List<Card>>
            {
                {FirstEpochCards[0], new List<Card> {FirstEpochCards[2], FirstEpochCards[3] }}, {FirstEpochCards[1], new List<Card> {FirstEpochCards[3], FirstEpochCards[4] }},
                {FirstEpochCards[2], new List<Card> {FirstEpochCards[5], FirstEpochCards[6] }}, {FirstEpochCards[3], new List<Card> {FirstEpochCards[6], FirstEpochCards[7] }},
                {FirstEpochCards[4], new List<Card> {FirstEpochCards[7], FirstEpochCards[8] }}, {FirstEpochCards[5], new List<Card> {FirstEpochCards[9], FirstEpochCards[10] }},
                {FirstEpochCards[6], new List<Card> {FirstEpochCards[10], FirstEpochCards[11] }}, {FirstEpochCards[7], new List<Card> {FirstEpochCards[11], FirstEpochCards[12] }},
                {FirstEpochCards[8], new List<Card> {FirstEpochCards[12], FirstEpochCards[13] }}, 
                {FirstEpochCards[9], new List<Card> {FirstEpochCards[14], FirstEpochCards[15] }}, {FirstEpochCards[10], new List<Card> {FirstEpochCards[15], FirstEpochCards[16] }},
                {FirstEpochCards[11], new List<Card> {FirstEpochCards[16], FirstEpochCards[17] }}, {FirstEpochCards[12], new List<Card> {FirstEpochCards[17], FirstEpochCards[18] }},
                {FirstEpochCards[13], new List<Card> {FirstEpochCards[18], FirstEpochCards[19] }}, {FirstEpochCards[14], new List<Card>()}, {FirstEpochCards[15], new List<Card>()},
                {FirstEpochCards[16], new List<Card>()}, {FirstEpochCards[17], new List<Card>()}, {FirstEpochCards[18], new List<Card>()}, {FirstEpochCards[19], new List<Card>()},

            };
            for (int i = 14; i < 20; i++)
            {
                FirstEpochCards[i].IsAvailable = true;
            }
            SecondEpochGraph = new Dictionary<Card, List<Card>>();
            ThirdEpochGraph = new Dictionary<Card, List<Card>>();
        }

        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void ChangeEpoch()
        {
            //TODO
        }
    }
}
