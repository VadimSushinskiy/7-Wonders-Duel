using _7_Wonders.Services;
using _7_Wonders.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _7_Wonders.Models.DbModels;
using Microsoft.IdentityModel.Tokens;


namespace _7_Wonders
{
    class MenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IWindowService _windowService;

        public RelayCommand StartCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand GoCommand { get; set; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }

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
        private int _pageNum;
        public bool[] Pages { get; set; }

        private string _firstPlayerName;
        public string FirstPlayerName 
        {
            get => _firstPlayerName;
            set
            {
                _firstPlayerName = value;
                OnPropertyChanged(nameof(FirstPlayerName));
            }
        }

        private string _secondPlayerName;
        public string SecondPlayerName
        {
            get => _secondPlayerName;
            set
            {
                _secondPlayerName = value;
                OnPropertyChanged(nameof(SecondPlayerName));
            }
        }

        private string _login = "";
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private bool _isMuted = true;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                OnPropertyChanged(nameof(IsMuted));
            }
        }
        public List<GameResults> GameResultsList { get; set; }

        public ObservableCollection<Visibility> PageVisibility { get; set; }

        private void OnOpenWindow()
        {
            _windowService.OpenWindow();
        }

        private void OnCloseWindow()
        { 
            _windowService.CloseWindow();
        }

        public MenuViewModel(IWindowService windowService)
        {
            FirstPlayerName = "Player1";
            SecondPlayerName = "Player2";
            _pageNum = 0;
            Pages = [true, false, false];
            PageVisibility = new ObservableCollection<Visibility> {Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed };
            PageVisibility[Mediator.PageId] = Visibility.Visible;
            IsMuted = Mediator.IsMuted;
            Mediator.PageId = 0;
            Login = Mediator.Login;
            GameResultsList = new();

            _windowService = windowService;
            ApplicationContext db = new ApplicationContext();

            if (!Login.IsNullOrEmpty())
            {
                GameResultsList = db.GameResults.Where(u => u.UserId == Mediator.UserId).ToList();
            }
            LogInCommand = new RelayCommand(obj =>
            {
                User user = db.Users.FirstOrDefault(u => u.Login == Login.Trim() && u.Password == Password.Trim());
                if (user == null)
                {
                    Help = "Не знайдено користувача з таким логіном і паролем";
                }
                else
                {
                    PageVisibility[0] = Visibility.Collapsed;
                    PageVisibility[2] = Visibility.Visible;
                    Help = "";
                    Mediator.UserId = user.Id;
                    Mediator.IsMuted = user.IsSoundMuted;
                    IsMuted = user.IsSoundMuted;
                    GameResultsList = db.GameResults.Where(u => u.UserId == Mediator.UserId).ToList();
                    OnPropertyChanged(nameof(GameResultsList));
                    OnPropertyChanged(nameof(PageVisibility));
                }
            }, obj =>
            {
                return Login.Trim() != string.Empty && Password.Trim() != string.Empty;
            });
            RegisterCommand = new RelayCommand(obj =>
            {
                if (db.Users.FirstOrDefault(u => u.Login == Login.Trim()) != null)
                {
                    Help = "Користувач з таким логіном вже існує";
                }
                else
                {
                    db.Users.Add(new User { Login = Login.Trim(), Password = Password.Trim(), IsSoundMuted = false });
                    db.SaveChanges();
                    PageVisibility[1] = Visibility.Collapsed;
                    PageVisibility[2] = Visibility.Visible;
                    Help = "";
                    GameResultsList = db.GameResults.ToList();
                    IsMuted = false;
                    Mediator.IsMuted = false;
                    Mediator.UserId = db.Users.FirstOrDefault(u => u.Login == Login.Trim()).Id;
                    OnPropertyChanged(nameof(GameResultsList));
                    OnPropertyChanged(nameof(PageVisibility));
                }
            }, obj =>
            {
                return Login.Trim() != string.Empty && Password.Trim() != string.Empty;
            });
            StartCommand = new RelayCommand(obj =>
            {
                Mediator.FirstPlayerName = FirstPlayerName.Trim();
                Mediator.SecondPlayerName = SecondPlayerName.Trim();
                Mediator.Login = Login.Trim();
                
                OnOpenWindow();
            }, obj =>
            {
                return FirstPlayerName.Trim() != string.Empty && SecondPlayerName.Trim() != string.Empty && FirstPlayerName != SecondPlayerName;
            });
            CloseWindowCommand = new RelayCommand(obj => OnCloseWindow());
            GoCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                for (int i = 0; i < PageVisibility.Count; i++)
                {
                    PageVisibility[i] = Visibility.Collapsed;
                }
                PageVisibility[idx] = Visibility.Visible;
                Help = "";
                if (idx == 0 || idx == 1)
                {
                    Login = "";
                    Password = "";
                    IsMuted = true;
                }
                else if (idx == 2)
                {
                    FirstPlayerName = "Player1";
                    SecondPlayerName = "Player2";
                }
                if (IsMuted != Mediator.IsMuted && idx != 0 && idx != 1)
                {
                    Mediator.IsMuted = IsMuted;
                    User user = db.Users.FirstOrDefault(u => u.Id == Mediator.UserId);
                    user.IsSoundMuted = IsMuted;
                    db.SaveChanges();
                }
                OnPropertyChanged(nameof(PageVisibility));
            });
            NextPageCommand = new RelayCommand(obj =>
            {
                Pages[_pageNum] = false;
                _pageNum++;
                if (_pageNum >= Pages.Length)
                {
                    _pageNum = 0;
                }
                Pages[_pageNum] = true;
                OnPropertyChanged(nameof(Pages));
            });
            PreviousPageCommand = new RelayCommand(obj =>
            {
                Pages[_pageNum] = false;
                _pageNum--;
                if (_pageNum < 0)
                {
                    _pageNum = Pages.Length - 1;
                }
                Pages[_pageNum] = true;
                OnPropertyChanged(nameof(Pages));
            });
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
