using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _7_Wonders.Services
{
    public class WindowService : IWindowService
    {
        public void OpenWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            Window window;
            if (currentWindow is Menu)
            {
                window = new MainWindow();
            }
            else
            {
                window = new Menu();
            }

            window.Show();
            currentWindow.Close();
        }

        public void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            if (window != null)
            {
                window.Close();
            }
        }
    }
}
