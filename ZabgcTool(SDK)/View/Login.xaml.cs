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
using ZabgcTool_SDK_.Model;

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            String host = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
         
            Helper.Settings settings = new ZabgcTool_SDK_.Helper.Settings().ReadSettings();
            if (settings.StayOnline == true && true == new Authorization().CheckUserUserWithThisName(settings.Name) && settings.ControlData == ip.ToString())
            {
                new Loader().Show();
                Close();
            }
            else
            {
                InitializeComponent();
            }
        }
        private void ToolBars_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            textpass.Visibility = Password.Password.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SignIn();
        }
        private void SignIn()
        {
            String host = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
            bool Autho = new Authorization(Logins.Text, Password.Password).Login();
            new ZabgcTool_SDK_.Helper.Settings().SetCheckStayOnline((bool)TGB.IsChecked, ip.ToString());
            new ZabgcTool_SDK_.Helper.Settings().SetName(Logins.Text);
            if (Autho == true)
            {
                WrongDataText.Visibility = Visibility.Collapsed;
                new Loader().Show();
                Close();
            }
            else
            {
                WrongDataText.Visibility = Visibility.Visible;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                SignIn();
            }
        }
    }
}
