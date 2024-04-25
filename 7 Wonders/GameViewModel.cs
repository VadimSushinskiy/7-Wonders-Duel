using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using _7_Wonders.Models;
using System.Windows.Media;

namespace _7_Wonders
{
    class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public RelayCommand ShuffleCommand { get; set; }
        public RelayCommand HandFlipCommand { get; set; }
        public RelayCommand BuyCommand { get; set; }
        public RelayCommand BuyWonderCommand { get; set; }
        public RelayCommand SellCommand { get; set; }
        public RelayCommand SelectCardCommand {  get; set; }
        public Wonder WonderToBuy { get; set; }
        public Card WonderCard { get; set; }

        public int[] BackCards = {2, 3, 4, 9, 10, 11, 12, 13};
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }

        private string _help;
        public string Help
        {
            get => _help;
            set
            {
                _help = value;
                OnPropertyChanged(nameof(Help));
            }
        }
        private Brush _helpColor;
        public Brush HelpColor
        {
            get => _helpColor;
            set
            {
                _helpColor = value;
                OnPropertyChanged(nameof(HelpColor));
            }
        }

        private int _firstPlayerCardsNumber = 0;
        public int FirstPlayerCardsNumber
        {
            get => _firstPlayerCardsNumber;
            set
            {
                _firstPlayerCardsNumber = value;
                OnPropertyChanged(nameof(FirstPlayerCardsNumber));
            }
        }
        private int _secondPlayerCardsNumber = 0;
        public int SecondPlayerCardsNumber
        {
            get => _secondPlayerCardsNumber;
            set
            {
                _secondPlayerCardsNumber = value;
                OnPropertyChanged(nameof(SecondPlayerCardsNumber));
            }
        }
        public ObservableCollection<string> FirstPlayerCards { get; set; } = new();
        public ObservableCollection<string> SecondPlayerCards { get; set; } = new();
        public Visibility StartButtonVisibility { get; set; } = Visibility.Visible;
        public Brush[] NameColors { get; set; } = { Brushes.Black, Brushes.Black };
        public Visibility[] PlayersInterfaceVisibility { get; set; } = { Visibility.Visible, Visibility.Collapsed };
        public Visibility[] FirstPlayerHandVisibility { get; set; } = {Visibility.Visible, Visibility.Collapsed};
        public Visibility[] SecondPlayerHandVisibility { get; set; } = {Visibility.Visible, Visibility.Collapsed};
        public ObservableCollection<string> CardsName { get; } = new();

        public ObservableCollection<string> WondersName { get; } = new();

        public ObservableCollection<Visibility> CardsVisibility { get; } = new();

        private Resources _firstResources;

        public Resources FirstResource
        {
            get => _firstResources;
            set
            {
                _firstResources = value;
                OnPropertyChanged(nameof(FirstResource));
            }
        }
        private Resources _secondResources;

        public Resources SecondResources
        {
            get => _secondResources;
            set
            {
                _secondResources = value;
                OnPropertyChanged(nameof(SecondResources));
            }
        }

        private int _war;

        public int War
        {
            get => _war;
            set
            {
                _war = value;
                OnPropertyChanged(nameof(War));
            }
        }

        private Visibility _WindowSelectingVisibility = Visibility.Collapsed;

        public Visibility WindowSelectingVisibility
        {
            get => _WindowSelectingVisibility;
            set
            {
                _WindowSelectingVisibility = value;
                OnPropertyChanged(nameof(WindowSelectingVisibility));
            }
        }
        public ObservableCollection<string> SelectingCardsNames { get; set; } = new();

        public string[] Backs = { @"Images\Cards\BackFirstEpoch.png", @"Images\Cards\BackSecondEpoch.png" , @"Images\Cards\BackThirdEpoch.png" };

        public GameViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"Images\Cards\BackFirstEpoch.png");
            }
            for (int i = 0; i < 30;  i++)
            {
                FirstPlayerCards.Add(@"Images/Cards/Transparent.png");
                SecondPlayerCards.Add(@"Images/Cards/Transparent.png");
            }
            for (int i = 0; i < 8; i++)
            {
                WondersName.Add(@"Images/Wonders/CircusMaximus.png");
            }
            for (int i = 0; i < 59; i++)
            {
                SelectingCardsNames.Add(@"Images/Cards/Transparent.png");
            }

            ShuffleCommand = new RelayCommand(obj =>
            {
                Game.InitializeGame();
                StartButtonVisibility = Visibility.Collapsed;
                FirstPlayerName = Game.FirstPlayer.Name;
                SecondPlayerName = Game.SecondPlayer.Name;
                FirstPlayerCardsNumber = 0;
                SecondPlayerCardsNumber = 0;
                ChangePlayer();
                for (int i = 0; i < Game.CardsList.Count; i++)
                {
                    if (BackCards.Contains(i)) continue;
                    CardsName[i] = @"Images/Cards/" + Game.CardsList[i].Name + ".png";
                }
                for (int i = 0; i < WondersName.Count; i++)
                {
                    WondersName[i] = @"Images/Wonders/" + Game.WondersList[i].Name + ".png";
                }
                War = 0;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                OnPropertyChanged(nameof(StartButtonVisibility));
                OnPropertyChanged(nameof(FirstPlayerName));
                OnPropertyChanged(nameof(SecondPlayerName));
                OnPropertyChanged(nameof(CardsName));
                OnPropertyChanged(nameof(WondersName));
            });

            BuyCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                CardsVisibility[idx] = Visibility.Collapsed;
                if (WonderToBuy != null && ((WonderToBuy.Effect == Wonder.WonderEffect.StealBrown && Game.CurrentPlayer.Opponent.BrownCards.Count > 0) ||
                (WonderToBuy.Effect == Wonder.WonderEffect.StealGray && Game.CurrentPlayer.Opponent.GrayCards.Count > 0) || (WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard && Game.DiscardedCards.Count > 0)))
                {
                    WonderCard = Game.CardsList[idx];
                    HelpColor = Brushes.Green;
                    switch (WonderToBuy.Effect)
                    {
                        case Wonder.WonderEffect.StealBrown:
                        {
                            for (int i = 0; i < Game.CurrentPlayer.Opponent.BrownCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"Images/Cards/" + Game.CurrentPlayer.Opponent.BrownCards[i].Name + ".png";
                            }
                            Help = "Оберіть карту. Ваш суперник буде змушений її скинути";
                            break;
                        }
                        case Wonder.WonderEffect.StealGray: 
                        {
                            for (int i = 0; i < Game.CurrentPlayer.Opponent.GrayCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"Images/Cards/" + Game.CurrentPlayer.Opponent.GrayCards[i].Name + ".png";
                            }
                            Help = "Оберіть карту. Ваш суперник буде змушений її скинути";
                            break;
                        }
                        case Wonder.WonderEffect.TakeDiscard:
                        {
                            for (int i = 0; i < Game.DiscardedCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"Images/Cards/" + Game.DiscardedCards[i].Name + ".png";
                            }
                            Help = "Оберіть одну з карт, скинутих під час гри. Ви отримаєте її собі у руку";
                            break;
                        }
                    }
                    OnPropertyChanged(nameof(SelectingCardsNames));
                    WindowSelectingVisibility = Visibility.Visible;
                }
                else
                {
                    if (WonderToBuy != null)
                    {
                        Game.CurrentPlayer.BuildWonder(WonderToBuy, Game.CardsList[idx]);
                        WonderToBuy = null;
                    }
                    else
                    {
                        AddCartToHand(Game.CardsList[idx]);
                        Game.CurrentPlayer.BuyCard(Game.CardsList[idx]);
                    }
                    UpdateField();
                }
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                return card.IsAvailable && (Game.CurrentPlayer.CheckPrice(card) != -1 || WonderToBuy != null);
            });

            SellCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.SellCard(Game.CardsList[idx]);
                CardsVisibility[idx] = Visibility.Collapsed;
                FirstResource = Game.FirstPlayer.Resource;
                SecondResources = Game.SecondPlayer.Resource;
                CheckBacks();
                OnPropertyChanged(nameof(CardsVisibility));
                ChangePlayer();
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                return (card.IsAvailable);
            });

            HandFlipCommand = new RelayCommand(obj =>
            {
                Visibility[] visibilities;
                visibilities = (Game.CurrentPlayer == Game.FirstPlayer) ? FirstPlayerHandVisibility : SecondPlayerHandVisibility;
                if (visibilities[0] == Visibility.Visible)
                {
                    visibilities[0] = Visibility.Collapsed;
                    visibilities[1] = Visibility.Visible;
                }
                else
                {
                    visibilities[0] = Visibility.Visible;
                    visibilities[1] = Visibility.Collapsed;
                }
                if (Game.CurrentPlayer == Game.FirstPlayer)
                {
                    OnPropertyChanged(nameof(FirstPlayerHandVisibility));
                }
                else
                {
                    OnPropertyChanged(nameof(SecondPlayerHandVisibility));
                }
            }, obj =>
            {
                return (Game.CurrentPlayer == Game.FirstPlayer && FirstPlayerCardsNumber > 15) || (Game.CurrentPlayer == Game.SecondPlayer && SecondPlayerCardsNumber > 15);
            });
            BuyWonderCommand = new RelayCommand(obj =>
            {
                Wonder wonder = Game.WondersList[int.Parse((string)obj)];
                if (wonder == WonderToBuy)
                {
                    WonderToBuy = null;
                    Help = string.Empty;
                }
                else
                {
                    WonderToBuy = wonder;
                    HelpColor = Brushes.Green;
                    Help = "Оберіть будь-яку доступну карту для побудови чуда";
                }
            }, obj =>
            {
                Wonder wonder = Game.WondersList[int.Parse((string)obj)];
                return Game.CurrentPlayer.CheckPrice(wonder) != -1 && !Game.CurrentPlayer.Wonders[wonder];
            });
            SelectCardCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                switch (WonderToBuy.Effect)
                {
                    case Wonder.WonderEffect.StealBrown:
                    {
                        Game.CurrentPlayer.ChosenCard = Game.CurrentPlayer.Opponent.BrownCards[idx];
                        for (int i = 0; i < Game.CurrentPlayer.Opponent.BrownCards.Count; i++)
                        {
                            SelectingCardsNames[i] = @"Images/Cards/Transparent.png";
                        }
                        break;
                    }
                    case Wonder.WonderEffect.StealGray:
                    {
                        Game.CurrentPlayer.ChosenCard = Game.CurrentPlayer.Opponent.GrayCards[idx];
                        for (int i = 0; i < Game.CurrentPlayer.Opponent.GrayCards.Count; i++)
                        {
                            SelectingCardsNames[i] = @"Images/Cards/Transparent.png";
                        }
                        break;
                    }
                    case Wonder.WonderEffect.TakeDiscard:
                    {
                        Game.CurrentPlayer.ChosenCard = Game.DiscardedCards[idx];
                        for (int i = 0; i < Game.DiscardedCards.Count; i++)
                        {
                            SelectingCardsNames[i] = @"Images/Cards/Transparent.png";
                        }
                        break;
                    }
                }
                WindowSelectingVisibility = Visibility.Collapsed;
                if (WonderToBuy.Effect == Wonder.WonderEffect.StealGray || WonderToBuy.Effect == Wonder.WonderEffect.StealBrown)
                {
                    if (Game.CurrentPlayer == Game.FirstPlayer)
                    {
                        for (int i = 0; i < SecondPlayerCards.Count; i++)
                        {
                            if (SecondPlayerCards[i].Contains(Game.FirstPlayer.ChosenCard.Name))
                            {
                                DeleteCardFromHand(SecondPlayerCards, i);
                                SecondPlayerCardsNumber--;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FirstPlayerCards.Count; i++)
                        {
                            if (FirstPlayerCards[i].Contains(Game.SecondPlayer.ChosenCard.Name))
                            {
                                DeleteCardFromHand(FirstPlayerCards, i);
                                FirstPlayerCardsNumber--;
                                break;
                            }
                        }
                    }
                }
                else if (WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard)
                {
                    AddCartToHand(Game.CurrentPlayer.ChosenCard);
                }
                Game.CurrentPlayer.BuildWonder(WonderToBuy, WonderCard);
                WonderToBuy = null;
                WonderCard = null;
                Game.CurrentPlayer.ChosenCard = null;
                UpdateField();
            }, obj =>
            {
                int idx = int.Parse((string)obj);
                switch (WonderToBuy.Effect)
                {
                    case Wonder.WonderEffect.StealBrown: return idx < Game.CurrentPlayer.Opponent.BrownCards.Count;
                    case Wonder.WonderEffect.StealGray: return idx < Game.CurrentPlayer.Opponent.GrayCards.Count;
                    case Wonder.WonderEffect.TakeDiscard: return idx < Game.DiscardedCards.Count;
                }
                return false;
            });
        }

        public void DeleteCardFromHand(ObservableCollection<string> hand, int idx)
        {
            for (int i = idx; i < hand.Count - 1; i++)
            {
                hand[i] = hand[i + 1];
            }
            hand[^1] = @"Images/Cards/Transparent.png";
        }

        public void AddCartToHand(Card card)
        {
            if (Game.CurrentPlayer == Game.FirstPlayer)
            {
                FirstPlayerCards[FirstPlayerCardsNumber] = @"Images/Cards/" + card.Name + ".png";
                FirstPlayerCardsNumber++;
                OnPropertyChanged(nameof(FirstPlayerCards));
            }
            else
            {
                SecondPlayerCards[SecondPlayerCardsNumber] = @"Images/Cards/" + card.Name + ".png";
                SecondPlayerCardsNumber++;
                OnPropertyChanged(nameof(SecondPlayerCards));
            }
        }

        public void CheckBacks()
        {
            bool flag = false;
            for (int i = 0; i < Game.CardsList.Count; i++)
            {
                if (Backs.Contains(CardsName[i]) && Game.CardsList[i].IsAvailable)
                {
                    CardsName[i] = @"Images\Cards\" + Game.CardsList[i].Name + ".png";
                    flag = true;
                }
            }
            if (flag) OnPropertyChanged(nameof(CardsName));
        }
        public void ChangePlayer()
        {
            if (Game.CurrentPlayer == Game.FirstPlayer)
            {
                NameColors[0] = Brushes.Green;
                NameColors[1] = Brushes.Black;
                PlayersInterfaceVisibility[0] = Visibility.Visible;
                PlayersInterfaceVisibility[1] = Visibility.Collapsed;
            }
            else
            {
                NameColors[0] = Brushes.Black;
                NameColors[1] = Brushes.Green;
                PlayersInterfaceVisibility[0] = Visibility.Collapsed;
                PlayersInterfaceVisibility[1] = Visibility.Visible;
            }
            OnPropertyChanged(nameof(PlayersInterfaceVisibility));
            OnPropertyChanged(nameof(NameColors));
        }

        public void UpdateField()
        {
            FirstResource = Game.FirstPlayer.Resource;
            SecondResources = Game.SecondPlayer.Resource;
            War = Game.WarPoints;
            Help = string.Empty;
            CheckBacks();
            ChangePlayer();
            OnPropertyChanged(nameof(CardsVisibility));
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
