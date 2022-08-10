using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для Loader.xaml
    /// </summary>
    public partial class Loader : Window
    {
        private int _type;
        public Loader(int Type)
        {
            InitializeComponent();
            _type = Type;
            LoadWindow(new Helper.Settings().ReadSettings().APIPath);
            
        }
        private async void LoadWindow(string Hostname)
        {
            var SplitedName = Hostname.Split('/');
            Hostname = SplitedName.FirstOrDefault(x => x.Contains(".ru"));
            if (Helper.Helper.CheckInternet(Hostname))
            {
                NotConnectedPopUp.Visibility = Visibility.Collapsed;
                LoadAnim.Visibility = Visibility.Visible;
                var Window = new MainWindow(_type);
                await Task.Delay(2000);
                Window.Show();
                Close();
            }
            else
            {
                this.Title = "Нет подключения к интернету";
                LoadAnim.Visibility = Visibility.Collapsed;
                NotConnectedPopUp.Visibility = Visibility.Visible;
                Reconnect.Click +=(s,e) =>
                {
                    LoadWindow(Hostname);
                   
                };
            }
         
        }
    }
}
