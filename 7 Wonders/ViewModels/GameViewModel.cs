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
using System.Windows.Controls;
using _7_Wonders.ViewModels;
using System.Windows.Forms;
using _7_Wonders.Services;



namespace _7_Wonders
{
    class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Models.DbModels.ApplicationContext Database { get; private set; }
        private IWindowService _windowService;
        public RelayCommand RestartCommand { get; set; }
        public RelayCommand HandFlipCommand { get; set; }
        public RelayCommand BuyCommand { get; set; }
        public RelayCommand BuyWonderCommand { get; set; }
        public RelayCommand SellCommand { get; set; }
        public RelayCommand SelectCardCommand {  get; set; }
        public RelayCommand SelectTokenCommand { get; set; }
        public RelayCommand PeekCommand { get; set; }
        public RelayCommand QiutCommand { get; set; }
        public byte EpochNumber { get; set; }
        public ObservableCollection<Visibility> EpochVisibility {  get; set; }
        public Wonder WonderToBuy { get; set; }
        public Card WonderCard { get; set; }

        public int[] BackCards = {2, 3, 4, 9, 10, 11, 12, 13, 26, 27, 28, 29, 30, 35, 36, 37, 42, 43, 44, 49, 50, 55, 56, 57};
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

        private Brush _peekColor;
        public Brush PeekColor
        {
            get => _peekColor;
            set
            {
                _peekColor = value;
                OnPropertyChanged(nameof(PeekColor));
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
        public ObservableCollection<string> FirstPlayerTokens { get; set; } = new();
        public ObservableCollection<string> SecondPlayerTokens { get; set; } = new();
        public ObservableCollection<string> FirstPlayerTokenToolTipEnable { get; set; } = new();
        public ObservableCollection<string> SecondPlayerTokenToolTipEnable { get; set; } = new();
        public ObservableCollection<string> FirstPlayerTokenToolTip { get; set; } = new();
        public ObservableCollection<string> SecondPlayerTokenToolTip { get; set; } = new();
        public ObservableCollection<string> AdditionalTokensToolTipEnable { get; set; } = new();
        public Visibility StartButtonVisibility { get; set; } = Visibility.Visible;
        public Brush[] NameColors { get; set; } = { Brushes.Black, Brushes.Black };
        public Visibility[] PlayersInterfaceVisibility { get; set; }
        public Visibility[] FirstPlayerHandVisibility { get; set; }
        public Visibility[] SecondPlayerHandVisibility { get; set; }
        public ObservableCollection<string> CardsName { get; set; } = new();

        public ObservableCollection<string> WondersName { get; set; } = new();
        public ObservableCollection<string> WondersHint { get; set; } = new();

        public ObservableCollection<Visibility> CardsVisibility { get; set; } = new();
        public ObservableCollection<string> TokensName { get; set; } = new();
        public ObservableCollection<string> TokensHints { get; set; } = new();
        public ObservableCollection<Visibility> TokensVisibility { get; set; } = new();
        

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
        private short _firstPlayerFame;

        public short FirstPlayerFame
        {
            get => _firstPlayerFame;
            set
            {
                _firstPlayerFame = value;
                OnPropertyChanged(nameof(FirstPlayerFame));
            }
        }
        private short _secondPlayerFame;

        public short SecondPlayerFame
        {
            get => _secondPlayerFame;
            set
            {
                _secondPlayerFame = value;
                OnPropertyChanged(nameof(SecondPlayerFame));
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

        private bool _selectingToken;
        public bool SelectingToken
        {
            get => _selectingToken;
            set
            {
                _selectingToken = value;
                OnPropertyChanged(nameof(SelectingToken));
            }
        }
        public ObservableCollection<string> SelectingCardsNames { get; set; } = new();
        public ObservableCollection<string> BuildedWonders {  get; set; } = new();
        public ObservableCollection<Brush> BorderArounWonderColor { get; set; } = new();

        public string[] Backs = { @"../Images/Cards/BackFirstEpoch.png", @"../Images/Cards/BackSecondEpoch.png", @"../Images/Cards/BackThirdEpoch.png" };


        public string WinnerName { get; set; }
        public string WinType { get; set; }
        public Brush WinColor { get; set; }
        public int[] FirstPlayerWinPoints { get; set; }
        public int[] SecondPlayerWinPoints { get; set; }
        public Visibility StatisticVisibility { get; set; }

        public GameViewModel()
        {
            Database = new();
            _windowService = new WindowService();

            FirstPlayerName = (Mediator.FirstPlayerName?.Length > 0) ? Mediator.FirstPlayerName : "Player1";
            SecondPlayerName = (Mediator.SecondPlayerName?.Length > 0) ? Mediator.SecondPlayerName : "Player2";

            StartGame();

            RestartCommand = new RelayCommand(obj =>
            {
                StartGame();
            });

            BuyCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                CardsVisibility[idx] = Visibility.Collapsed;
                if (WonderToBuy != null && ((WonderToBuy.Effect == Wonder.WonderEffect.StealBrown && Game.CurrentPlayer.Opponent.BrownCards.Count > 0) ||
                (WonderToBuy.Effect == Wonder.WonderEffect.StealGray && Game.CurrentPlayer.Opponent.GrayCards.Count > 0) ||
                (WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard && Game.DiscardedCards.Count > 0) || 
                (WonderToBuy.Effect == Wonder.WonderEffect.Token && TokensVisibility.Contains(Visibility.Visible))))
                {
                    WonderCard = Game.CardsList[idx];
                    HelpColor = Brushes.Green;
                    switch (WonderToBuy.Effect)
                    {
                        case Wonder.WonderEffect.StealBrown:
                        {
                            for (int i = 0; i < Game.CurrentPlayer.Opponent.BrownCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"../Images/Cards/" + Game.CurrentPlayer.Opponent.BrownCards[i].Name + ".png";
                            }
                            Help = "Оберіть карту. Ваш суперник буде змушений її скинути";
                            break;
                        }
                        case Wonder.WonderEffect.StealGray: 
                        {
                            for (int i = 0; i < Game.CurrentPlayer.Opponent.GrayCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"../Images/Cards/" + Game.CurrentPlayer.Opponent.GrayCards[i].Name + ".png";
                            }
                            Help = "Оберіть карту. Ваш суперник буде змушений її скинути";
                            break;
                        }
                        case Wonder.WonderEffect.TakeDiscard:
                        {
                            for (int i = 0; i < Game.DiscardedCards.Count; i++)
                            {
                                SelectingCardsNames[i] = @"../Images/Cards/" + Game.DiscardedCards[i].Name + ".png";
                            }
                            Help = "Оберіть одну з карт, скинутих під час гри. Ви отримаєте її собі у руку";
                            break;
                        }
                        case Wonder.WonderEffect.Token:
                        {
                            for (int i = 0; i < Game.AdditionalTokens.Count; i++)
                            {
                                SelectingCardsNames[i] = @"../Images/Tokens/" + Game.AdditionalTokens[i].Name + ".png";
                                AdditionalTokensToolTipEnable[i] = "True";
                            }
                            Help = "Оберіть один з трьох жетонів, які не беруть участь у грі";
                            break;
                        }
                    }
                    OnPropertyChanged(nameof(SelectingCardsNames));
                    OnPropertyChanged(nameof(AdditionalTokensToolTipEnable));
                    WindowSelectingVisibility = Visibility.Visible;
                    BuildedWonders[Game.WondersList.IndexOf(WonderToBuy)] = Backs[EpochNumber - 1];
                    HideBorder();
                    OnPropertyChanged(nameof(BuildedWonders));
                }
                else
                {
                    if (WonderToBuy != null)
                    {
                        Game.CurrentPlayer.BuildWonder(WonderToBuy, Game.CardsList[idx]);
                        BuildedWonders[Game.WondersList.IndexOf(WonderToBuy)] = Backs[EpochNumber - 1];
                        WonderToBuy = null;
                        HideBorder();
                        OnPropertyChanged(nameof(BuildedWonders));
                        UpdateField();
                    }
                    else if (Game.CardsList[idx] is GreenCard greenCard && Game.CurrentPlayer.Symbols[(int)greenCard.Symbol])
                    {
                        SelectingToken = true;
                        WonderCard = Game.CardsList[idx];
                        HelpColor = Brushes.Green;
                        Help = "Ви зібрали два однакових наукових символа. В нагороду оберіть один з жетонів у верхній частині екрана";
                    }
                    else
                    {
                        AddCartToHand(Game.CardsList[idx]);
                        Game.CurrentPlayer.BuyCard(Game.CardsList[idx]);
                        UpdateField();
                    }
                }
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                if (SelectingToken)
                {
                    return false;
                }
                if (!card.IsAvailable)
                {
                    HelpColor = Brushes.Red;
                    Help = "Ви не можете обрати карту, яка заблокована іншими картами";
                    return false;
                }
                else if (Game.CurrentPlayer.CheckPrice(card) == -1 && WonderToBuy == null)
                {
                    HelpColor = Brushes.Red;
                    Help = "У вас не вистачає ресурсів для покупци цієї карти";
                    return false;
                }
                return true;
            });

            SellCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.SellCard(Game.CardsList[idx]);
                CardsVisibility[idx] = Visibility.Collapsed;
                UpdateField();
            }, obj =>
            {
                Card card = Game.CardsList[int.Parse((string)obj)];
                if (SelectingToken)
                {
                    return false;
                }
                if (WonderToBuy != null)
                {
                    return false;
                }
                if (!card.IsAvailable) 
                {
                    HelpColor = Brushes.Red;
                    Help = "Ви не можете обрати карту, яка заблокована іншими картами";
                    return false;
                }
                return true;
            });

            HandFlipCommand = new RelayCommand(obj =>
            {
                Visibility[] visibilities;
                visibilities = (PlayersInterfaceVisibility[0] == Visibility.Visible) ? FirstPlayerHandVisibility : SecondPlayerHandVisibility;
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
                if (PlayersInterfaceVisibility[0] == Visibility.Visible)
                {
                    OnPropertyChanged(nameof(FirstPlayerHandVisibility));
                }
                else
                {
                    OnPropertyChanged(nameof(SecondPlayerHandVisibility));
                }
            }, obj =>
            {
                return (PlayersInterfaceVisibility[0] == Visibility.Visible && (FirstPlayerCardsNumber > 15 || FirstPlayerHandVisibility[1] == Visibility.Visible)) ||
                (PlayersInterfaceVisibility[1] == Visibility.Visible && (SecondPlayerCardsNumber > 15 || SecondPlayerHandVisibility[1] == Visibility.Visible));
            });

            BuyWonderCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                Wonder wonder = Game.WondersList[idx];
                HideBorder();
                if (wonder == WonderToBuy)
                {
                    WonderToBuy = null;
                    Help = string.Empty;
                }
                else
                {
                    WonderToBuy = wonder;
                    BorderArounWonderColor[idx] = Brushes.Green;
                    OnPropertyChanged(nameof(BorderArounWonderColor));
                    HelpColor = Brushes.Green;
                    Help = "Оберіть будь-яку доступну карту для побудови дива";
                }
            }, obj =>
            {
                Wonder wonder = Game.WondersList[int.Parse((string)obj)];
                if (SelectingToken)
                {
                    return false;
                }
                if(!Game.CurrentPlayer.Wonders.TryGetValue(wonder, out bool tmp))
                {
                    HelpColor = Brushes.Red;
                    Help = "Ви не можете побудувати диво суперника";
                    return false;
                }
                if (Game.CurrentPlayer.Wonders[wonder])
                {
                    HelpColor = Brushes.Red;
                    Help = "Це диво вже було побудовано";
                    return false;
                }
                else if (!Game.WonderIsAvailable)
                {
                    HelpColor = Brushes.Red;
                    Help = "Сім див вже було побудовано. Ви не можете побудувати восьме";
                    return false;
                }
                else if (Game.CurrentPlayer.CheckPrice(wonder) == -1)
                {
                    HelpColor = Brushes.Red;
                    Help = "Недостатньо ресурсів для побудови цього дива";
                    return false;
                }
                return true;
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
                            SelectingCardsNames[i] = @"../Images/Cards/Transparent.png";
                        }
                        break;
                    }
                    case Wonder.WonderEffect.StealGray:
                    {
                        Game.CurrentPlayer.ChosenCard = Game.CurrentPlayer.Opponent.GrayCards[idx];
                        for (int i = 0; i < Game.CurrentPlayer.Opponent.GrayCards.Count; i++)
                        {
                            SelectingCardsNames[i] = @"../Images/Cards/Transparent.png";
                        }
                        break;
                    }
                    case Wonder.WonderEffect.TakeDiscard:
                    {
                        Game.CurrentPlayer.ChosenCard = Game.DiscardedCards[idx];
                        for (int i = 0; i < Game.DiscardedCards.Count; i++)
                        {
                            SelectingCardsNames[i] = @"../Images/Cards/Transparent.png";
                        }
                        break;
                    }
                    case Wonder.WonderEffect.Token:
                    {
                        Game.CurrentPlayer.ChosenToken = Game.AdditionalTokens[idx];
                        for (int i = 0; i < Game.AdditionalTokens.Count; i++)
                        {
                            SelectingCardsNames[i] = @"../Images/Cards/Transparent.png";
                            AdditionalTokensToolTipEnable[i] = "False";
                            OnPropertyChanged(nameof(AdditionalTokensToolTipEnable));
                        }
                        AddToken();
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
                else if (WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard && !(Game.CurrentPlayer.ChosenCard is GreenCard card && Game.CurrentPlayer.Symbols[(int)card.Symbol]))
                {
                    AddCartToHand(Game.CurrentPlayer.ChosenCard);
                }
                if (WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard && Game.CurrentPlayer.ChosenCard is GreenCard greenCard && Game.CurrentPlayer.Symbols[(int)greenCard.Symbol])
                {
                    SelectingToken = true;
                    HelpColor = Brushes.Green;
                    Help = "Ви зібрали два однакових наукових символа. В нагороду оберіть один з жетонів у верхній частині екрана";
                }
                else
                {
                    Game.CurrentPlayer.BuildWonder(WonderToBuy, WonderCard);
                    WonderToBuy = null;
                    WonderCard = null;
                    Game.FirstPlayer.ChosenCard = null;
                    Game.FirstPlayer.ChosenToken = null;
                    Game.SecondPlayer.ChosenCard = null;
                    Game.SecondPlayer.ChosenToken = null;
                    UpdateField();
                }
                
            }, obj =>
            {
                int idx = int.Parse((string)obj);
                switch (WonderToBuy.Effect)
                {
                    case Wonder.WonderEffect.StealBrown: return idx < Game.CurrentPlayer.Opponent.BrownCards.Count;
                    case Wonder.WonderEffect.StealGray: return idx < Game.CurrentPlayer.Opponent.GrayCards.Count;
                    case Wonder.WonderEffect.TakeDiscard: return idx < Game.DiscardedCards.Count;
                    case Wonder.WonderEffect.Token: return idx < Game.AdditionalTokens.Count;
                }
                return false;
            });

            SelectTokenCommand = new RelayCommand(obj =>
            {
                Card card = (WonderToBuy != null && WonderToBuy.Effect == Wonder.WonderEffect.TakeDiscard) ? Game.CurrentPlayer.ChosenCard : WonderCard;
                int idx = int.Parse((string)obj);
                Game.CurrentPlayer.ChosenToken = Game.GameTokens[idx];
                AddCartToHand(card);
                AddToken();
                if (WonderToBuy == null || WonderToBuy.Effect != Wonder.WonderEffect.TakeDiscard)
                {
                    Game.CurrentPlayer.BuyCard(WonderCard);
                }
                else
                {
                    Game.CurrentPlayer.BuildWonder(WonderToBuy, WonderCard);
                    WonderToBuy = null;
                    Game.FirstPlayer.ChosenCard = null;
                    Game.SecondPlayer.ChosenCard = null;
                }
                Game.FirstPlayer.ChosenToken = null;
                Game.SecondPlayer.ChosenToken = null;
                WonderCard = null;
                SelectingToken = false;
                TokensVisibility[idx] = Visibility.Collapsed;
                UpdateField();
                OnPropertyChanged(nameof(TokensVisibility));

            }, obj =>
            {
                return SelectingToken;
            });

            PeekCommand = new RelayCommand(obj =>
            {
                for (int i = 0; i < PlayersInterfaceVisibility.Length; i++)
                {
                    if (PlayersInterfaceVisibility[i] == Visibility.Visible) PlayersInterfaceVisibility[i] = Visibility.Collapsed;
                    else PlayersInterfaceVisibility[i] = Visibility.Visible;
                }
                if (PeekColor == Brushes.Transparent) PeekColor = Brushes.Green;
                else PeekColor = Brushes.Transparent;
                OnPropertyChanged(nameof(PlayersInterfaceVisibility));
            });

            QiutCommand = new RelayCommand(obj =>
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Ви впевені, що хочете вийти з гри?", "Quit", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Mediator.PageId = 2;
                    _windowService.OpenWindow();
                }
            });
        }

        public void StartGame()
        {
            FirstPlayerCards = new();
            SecondPlayerCards = new();
            FirstPlayerTokens = new();
            SecondPlayerTokens = new();
            FirstPlayerTokenToolTipEnable = new();
            SecondPlayerTokenToolTipEnable = new();
            FirstPlayerTokenToolTip = new();
            SecondPlayerTokenToolTip = new();
            AdditionalTokensToolTipEnable = new();
            CardsName = new();
            WondersName = new();
            WondersHint = new();
            TokensName = new();
            TokensHints = new();
            CardsVisibility = new();
            TokensVisibility = new();
            SelectingCardsNames = new();
            BuildedWonders = new();
            BorderArounWonderColor = new();
            EpochNumber = 1;
            EpochVisibility = new ObservableCollection<Visibility> { Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed };

            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"../Images/Cards/BackFirstEpoch.png");
            }
            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"../Images/Cards/BackSecondEpoch.png");
            }
            for (int i = 0; i < 20; i++)
            {
                CardsVisibility.Add(Visibility.Visible);
                CardsName.Add(@"../Images/Cards/BackThirdEpoch.png");
            }

            for (int i = 0; i < 30; i++)
            {
                FirstPlayerCards.Add(@"../Images/Cards/Transparent.png");
                SecondPlayerCards.Add(@"../Images/Cards/Transparent.png");
            }

            for (int i = 0; i < 8; i++)
            {
                WondersName.Add(@"../Images/Wonders/CircusMaximus.png");
                WondersHint.Add(string.Empty);
                BuildedWonders.Add(@"../Images/Cards/Transparent.png");
                BorderArounWonderColor.Add(Brushes.Transparent);
                TokensName.Add(@"../Images/Tokens/Law.png");
                TokensHints.Add(string.Empty);
            }

            for (int i = 0; i < 59; i++)
            {
                SelectingCardsNames.Add(@"../Images/Cards/Transparent.png");
            }

            for (int i = 0; i < 5; i++)
            {
                TokensVisibility.Add(Visibility.Visible);
            }
            for (int i = 0; i < 6; i++)
            {
                FirstPlayerTokens.Add(@"../Images/Cards/Transparent.png");
                SecondPlayerTokens.Add(@"../Images/Cards/Transparent.png");
                FirstPlayerTokenToolTipEnable.Add("False");
                SecondPlayerTokenToolTipEnable.Add("False");
                FirstPlayerTokenToolTip.Add(string.Empty);
                SecondPlayerTokenToolTip.Add(string.Empty);
            }
            for (int i = 0; i < 3; i++)
            {
                AdditionalTokensToolTipEnable.Add("False");
            }

            PeekColor = Brushes.Transparent;
            PlayersInterfaceVisibility = [Visibility.Visible, Visibility.Collapsed];
            FirstPlayerHandVisibility = [Visibility.Visible, Visibility.Collapsed];
            SecondPlayerHandVisibility = [Visibility.Visible, Visibility.Collapsed];

            WinnerName = "";
            WinType = "";
            FirstPlayerWinPoints = new int[9];
            SecondPlayerWinPoints = new int[9];
            WinColor = Brushes.Purple;
            StatisticVisibility = Visibility.Collapsed;


            Game.InitializeGame();


            Game.FirstPlayer.Name = FirstPlayerName;
            Game.SecondPlayer.Name = SecondPlayerName;
            FirstPlayerCardsNumber = 0;
            SecondPlayerCardsNumber = 0;
            ChangePlayer();

            for (int i = 0; i < Game.CardsList.Count; i++)
            {
                if (BackCards.Contains(i)) continue;
                CardsName[i] = @"../Images/Cards/" + Game.CardsList[i].Name + ".png";
            }
            for (int i = 0; i < WondersName.Count; i++)
            {
                WondersName[i] = @"../Images/Wonders/" + Game.WondersList[i].Name + ".png";
                WondersHint[i] = Game.WondersList[i].Hint;
            }
            for (int i = 0; i < 5; i++)
            {
                TokensName[i] = @"../Images/Tokens/" + Game.GameTokens[i].Name + ".png";
                TokensHints[i] = Game.GameTokens[i].Hint;
            }
            for (int i = 5; i < 8; i++)
            {
                TokensName[i] = @"../Images/Tokens/" + Game.AdditionalTokens[i - 5].Name + ".png";
                TokensHints[i] = Game.AdditionalTokens[i - 5].Hint;
            }

            War = 0;
            FirstResource = Game.FirstPlayer.Resource;
            SecondResources = Game.SecondPlayer.Resource;
            FirstPlayerFame = Game.FirstPlayer.Fame;
            SecondPlayerFame = Game.SecondPlayer.Fame;

            OnPropertyChanged(nameof(StartButtonVisibility));
            OnPropertyChanged(nameof(FirstPlayerName));
            OnPropertyChanged(nameof(SecondPlayerName));
            OnPropertyChanged(nameof(CardsName));
            OnPropertyChanged(nameof(TokensName));
            OnPropertyChanged(nameof(TokensHints));
            OnPropertyChanged(nameof(TokensVisibility));
            OnPropertyChanged(nameof(WondersName));
            OnPropertyChanged(nameof(WondersHint));
            OnPropertyChanged(nameof(CardsVisibility));
            OnPropertyChanged(nameof(FirstPlayerCards));
            OnPropertyChanged(nameof(SecondPlayerCards));
            OnPropertyChanged(nameof(EpochVisibility));
            OnPropertyChanged(nameof(BuildedWonders));
            OnPropertyChanged(nameof(SelectingCardsNames));
            OnPropertyChanged(nameof(FirstPlayerTokens));
            OnPropertyChanged(nameof(SecondPlayerTokens));
            OnPropertyChanged(nameof(FirstPlayerTokenToolTip));
            OnPropertyChanged(nameof(SecondPlayerTokenToolTip));
            OnPropertyChanged(nameof(FirstPlayerTokenToolTipEnable));
            OnPropertyChanged(nameof(SecondPlayerTokenToolTipEnable));
            OnPropertyChanged(nameof(AdditionalTokensToolTipEnable));
        }

        public void DeleteCardFromHand(ObservableCollection<string> hand, int idx)
        {
            for (int i = idx; i < hand.Count - 1; i++)
            {
                hand[i] = hand[i + 1];
            }
            hand[^1] = @"../Images/Cards/Transparent.png";
        }

        public void AddCartToHand(Card card)
        {
            if (Game.CurrentPlayer == Game.FirstPlayer)
            {
                FirstPlayerCards[FirstPlayerCardsNumber] = @"../Images/Cards/" + card.Name + ".png";
                FirstPlayerCardsNumber++;
                OnPropertyChanged(nameof(FirstPlayerCards));
            }
            else
            {
                SecondPlayerCards[SecondPlayerCardsNumber] = @"../Images/Cards/" + card.Name + ".png";
                SecondPlayerCardsNumber++;
                OnPropertyChanged(nameof(SecondPlayerCards));
            }
        }

        public void AddToken()
        {
            int idx = Game.CurrentPlayer.Tokens.Count;
            if (Game.CurrentPlayer == Game.FirstPlayer)
            {
                FirstPlayerTokens[idx] = @"../Images/Tokens/" + Game.CurrentPlayer.ChosenToken.Name + ".png";
                FirstPlayerTokenToolTipEnable[idx] = "True";
                FirstPlayerTokenToolTip[idx] = Game.CurrentPlayer.ChosenToken.Hint;
                OnPropertyChanged(nameof(FirstPlayerTokenToolTipEnable));
                OnPropertyChanged(nameof(FirstPlayerTokenToolTip));
                OnPropertyChanged(nameof(FirstPlayerTokens));
            }
            else
            {
                SecondPlayerTokens[idx] = @"../Images/Tokens/" + Game.CurrentPlayer.ChosenToken.Name + ".png";
                SecondPlayerTokenToolTipEnable[idx] = "True";
                SecondPlayerTokenToolTip[idx] = Game.CurrentPlayer.ChosenToken.Hint;
                OnPropertyChanged(nameof(SecondPlayerTokenToolTipEnable));
                OnPropertyChanged(nameof(SecondPlayerTokenToolTip));
                OnPropertyChanged(nameof(SecondPlayerTokens));
            }
        } 
        public void HideBorder()
        {
            for (int i = 0; i < 8; i++)
            {
                BorderArounWonderColor[i] = Brushes.Transparent;
            }
            OnPropertyChanged(nameof(BorderArounWonderColor));
        }

        public void CheckBacks()
        {
            bool flag = false;
            for (int i = 0; i < Game.CardsList.Count; i++)
            {
                if (Backs.Contains(CardsName[i]) && Game.CardsList[i].IsAvailable)
                {
                    CardsName[i] = @"../Images/Cards/" + Game.CardsList[i].Name + ".png";
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
            FirstPlayerFame = Game.FirstPlayer.Fame;
            SecondPlayerFame = Game.SecondPlayer.Fame;
            War = Game.WarPoints;
            Help = string.Empty;
            PeekColor = Brushes.Transparent;
            CheckBacks();
            ChangePlayer();
            OnPropertyChanged(nameof(CardsVisibility));
            if (EpochNumber < Game.Epoch)
            {
                EpochVisibility[EpochNumber - 1] = Visibility.Collapsed;
                EpochNumber = Game.Epoch;
                EpochVisibility[EpochNumber - 1] = Visibility.Visible;
                OnPropertyChanged(nameof(EpochVisibility));
                if (EpochNumber == 4)
                {
                    EndGame();
                }
            }
        }

        public void EndGame()
        {
            WinnerName = Game.Winner?.Name ?? "Нічья"; 
            Game.GameEnding end = Game.Ending;
            switch (end)
            {
                case Game.GameEnding.Science:
                {
                    WinType = "Наукова Перемога!";
                    WinColor = Brushes.Green;
                    break;
                }
                case Game.GameEnding.War:
                {
                    WinType = "Військова Перемога!";
                    WinColor = Brushes.Red;
                    break;
                }
                case Game.GameEnding.Standard:
                {
                    WinType = "Цивільна Перемога!";
                    WinColor = Brushes.Purple;
                    CalcucalatePoints(Game.FirstPlayer, FirstPlayerWinPoints);
                    CalcucalatePoints(Game.SecondPlayer, SecondPlayerWinPoints);
                    StatisticVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(FirstPlayerWinPoints));
                    OnPropertyChanged(nameof(SecondPlayerWinPoints));
                    break;
                }
            }
            OnPropertyChanged(nameof(WinnerName));
            OnPropertyChanged(nameof(WinType));
            OnPropertyChanged(nameof(WinColor));
            OnPropertyChanged(nameof(StatisticVisibility));
            Database.GameResults.Add(new Models.DbModels.GameResults { FirstPlayerName = FirstPlayerName, SecondPlayerName = SecondPlayerName,
                WinType = WinType.Remove(WinType.Length - 1),  Winner = WinnerName, UserId = Mediator.UserId});
            Database.SaveChanges();
        }

        public void CalcucalatePoints(Player player, int[] pointArray)
        {
            int count = 0;
            for (int i = 0; i < player.BlueCards.Count; i++)
            {
                count += player.BlueCards[i].Fame;
            }
            pointArray[0] = count;
            count = 0;
            for (int i = 0; i < player.GreenCards.Count; i++)
            {
                count += player.GreenCards[i].Fame;
            }
            pointArray[1] = count;
            count = 0;
            for (int i = 0; i < player.YellowCards.Count; i++)
            {
                count += player.YellowCards[i].Fame;
            }
            pointArray[2] = count;
            count = 0;
            foreach (Wonder wonder in player.Wonders.Keys)
            {
                if (player.Wonders[wonder])
                {
                    count += wonder.Fame;
                }
            }
            pointArray[4] = count;
            count = 0;
            if (player.TokenEffects[Token.TokenEffect.Math])
            {
                count += 3 * player.Tokens.Count;
            }
            for (int i = 0; i<player.Tokens.Count; i++)
            {
                count += player.Tokens[i].Fame;
            }
            pointArray[5] = count;
            count = player.Resource.Gold / 3;
            pointArray[6] = count;
            count = 0;
            if (player == Game.FirstPlayer)
            {
                if (Game.WarPoints >= 6)
                {
                    count = 10;
                }
                else if (Game.WarPoints >= 3)
                {
                    count = 5;
                }
                else if (Game.WarPoints >= 1)
                {
                    count = 2;
                }
            }
            else
            {
                if (Game.WarPoints <= -6)
                {
                    count = 10;
                }
                else if (Game.WarPoints <= -3)
                {
                    count = 5;
                }
                else if (Game.WarPoints <= -1)
                {
                    count = 2;
                }
            }
            pointArray[7] = count;
            pointArray[3] = player.WinningPoints - pointArray.Sum();
            pointArray[8] = player.WinningPoints;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
