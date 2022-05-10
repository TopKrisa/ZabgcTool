using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings(Window Owner)
        {
            InitializeComponent();
            var settings = new Helper.Settings().ReadSettings();
            FTPaddres.Text = settings.Addres;
            FTPname.Text = settings.Login;
            FTPpassword.Password = settings.Password;
            AdminName.Text = settings.AdminName;
            TGB.IsChecked = settings.StayOnline;
            Autrn.IsChecked = settings.Autorun;
            API.Text = settings.APIPath;

            ShedulePath.Text = settings.ShedulePath;
            //  new Model.Settings(settings.Addres, settings.Login, settings.Password, true, settings.AdminName, ShedulePath.Text,settings.Name,).SaveSettings();
            Save.Click += (s, e) =>
            {
                new Helper.Settings(FTPaddres.Text, FTPname.Text, FTPpassword.Password, TGB.IsChecked.Value, AdminName.Text, ShedulePath.Text, settings.Name, Autrn.IsChecked.Value, API.Text, settings.ControlData).SaveSettings();
                Close();
            };
            Exit.Click += (s, e) =>
              {
                  new Helper.Settings().SetCheckStayOnline(false,"");
                  new Login().Show();
                  Owner.Close();
                  Close();
              };
            DeleteFiles.Click += (s, e) =>
            {
                var DirectoryCache = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\files");
                foreach (var file in DirectoryCache)
                {
                    File.Delete(file);
                }
                Activate();
            };
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void butns_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void butn_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ShedulePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start($"http://zabgc.zabedu.ru");
        }


    }
}
