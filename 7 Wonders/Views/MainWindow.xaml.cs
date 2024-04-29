using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _7_Wonders;

namespace _7_Wonders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer _mediaPlayer;
        public MainWindow()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectoryPath = System.IO.Path.GetDirectoryName(executableFilePath);
            string audioFilePath = System.IO.Path.Combine(executableDirectoryPath, "Music/GameMusic.mp3");

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += Media_Ended;
            _mediaPlayer.Open(new Uri(audioFilePath));
            _mediaPlayer.Volume = 0.01;

            InitializeComponent();
            _mediaPlayer.Play();
        }
        private void Media_Ended(object sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.FromMilliseconds(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.IsMuted = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.IsMuted = false;
        }
    }
}