using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ZabgcTool_SDK_.APIKeys.Core;
using ZabgcTool_SDK_.FTP;
using ZabgcTool_SDK_.View.Pages;

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        APIRequest<Notify> API = new APIRequest<Notify>(APIKeys.Core.DataTableNames.Tables.Notify, APIKeys.Keys.Api.Admin);
        List<Notify> Notifies = new List<Notify>();
        private int Count = 0;
        public MainWindow()
        {

            InitializeComponent();
            //  LoadNotify();
            MainWindowFrame.Navigate(new AdminPanel(NameOfPage, this));
            Control.Focus();
            Authomatic.Click += (s, e) =>
             {
                 MainWindowFrame.Navigate(new RemotePCs(NameOfPage));
             };

            Settings.Click += (s, e) =>
            {
                if (!Helper.Helper.IsWindowOpen<Settings>())
                {
                    new Settings(this).Show();
                }
            };
            Discous.Click += (s, e) =>
            {
                MainWindowFrame.Navigate(new Discous(MainWindowFrame, NameOfPage));

            };

            //TrayMain.Click += (s, e) =>
            //{
            //    MainWindowFrame.Navigate(new AdminPanel(NameOfPage));
            //    MainTray.Visibility = Visibility.Collapsed;
            //    MainTray.TrayPopup.Visibility = Visibility.Collapsed;
            //    TrayPopup.Visibility = Visibility.Collapsed;
            //    Show();
            //};
            //TrayClose.Click += (s, e) =>
            // {
            //     Environment.Exit(0);
            // };
            //TraySettings.Click += (s, e) =>
            //{

            //    TrayPopup.Visibility = Visibility.Collapsed;
            //    new Settings(this).Show();
            //};
            MainTray.TrayMouseDoubleClick += (s, e) =>
              {
                  MainTray.Visibility = Visibility.Collapsed;
                  //TrayPopup.Visibility = Visibility.Collapsed;
                  Show();
              };
            Activated += async (s, e) =>
             {
                 LoadNotify();
             };
            FTP.Click += (s, e) =>
            {
                NameOfPage.Visibility = Visibility.Collapsed;
                MainWindowFrame.Navigate(new MainFTP(this));
            };
            FocusableChanged += (s, e) =>
              {
                  NotifyPopup.Visibility = Visibility.Collapsed;
              };
            Control.Click += (s, e) =>
            {
                NameOfPage.Visibility = Visibility.Collapsed;
                NotifyPopup.Visibility = Visibility.Collapsed;
                MainWindowFrame.Navigate(new AdminPanel(NameOfPage, this));
            };
            Notify.Click += async (s, e) =>
            {


                NotifyPopup.Visibility = Visibility.Visible;
                Notifies = await API.GetData();
                NotifyList.ItemsSource = Notifies.OrderBy(x => x.IsSeen == 0).Reverse();
                await API.DeleteData(0);
                await API.EditData(null, 0);
                IssetNotify.Visibility = Visibility.Hidden;
                NotifyCount.Visibility = Visibility.Hidden;
                await API.DeleteData(0);

            };


            Loaded += (s, e) =>
          {
              //   LoadNotify();
          };

            Timer timer = new Timer();
            timer.Interval = 30000;
            timer.Start();
            timer.Tick += (s, e) =>
           {

               if (Count <= Notifies.Count)
               {
                   if (Notifies.Count(x => x.IsSeen == 0) == 0)
                   {
                       IssetNotify.Visibility = Visibility.Hidden;
                       NotifyCount.Visibility = Visibility.Hidden;
                   }
                   else
                   {
                       Count = Notifies.Count(x => x.IsSeen == 0);
                       NotifyCount.Text = Notifies.Count(x => x.IsSeen == 0).ToString();
                       IssetNotify.Visibility = Visibility.Visible;
                       NotifyCount.Visibility = Visibility.Hidden;
                   }
               }
               else
               {
                   IssetNotify.Visibility = Visibility.Hidden;
                   NotifyCount.Visibility = Visibility.Hidden;
               }

           };
        }

        private async void LoadNotify()
        {
            Notifies = await API.GetData();
            NotifyList.ItemsSource = Notifies.OrderBy(x => x.IsSeen == 0).Reverse();
            NotifyCount.Text = Notifies.Count(x => x.IsSeen == 0).ToString();
            Count = Convert.ToInt32(NotifyCount.Text);
            if (Count == 0)
            {
                IssetNotify.Visibility = Visibility.Hidden;
                NotifyCount.Visibility = Visibility.Hidden;
            }

        }
        private void butns_Click(object sender, RoutedEventArgs e)
        {
            MainTray.Visibility = Visibility.Visible;
            Hide();

        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

            NotifyPopup.Visibility = Visibility.Collapsed;
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();

            }
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var notify = (Notify)(sender as StackPanel).DataContext;
            switch (notify.Type)
            {
                case "comment":
                    MainWindowFrame.Navigate(new CommPage(NameOfPage));
                    break;
                case "question":
                    MainWindowFrame.Navigate(new QuestionsPage(NameOfPage));
                    break;
                default:
                    break;
            }
            NotifyPopup.Visibility = Visibility.Collapsed;
        }
        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private async void IssetNotify_MouseDown(object sender, MouseButtonEventArgs e)
        {

            NotifyPopup.Visibility = Visibility.Visible;
            Notifies = await API.GetData();
            NotifyList.ItemsSource = Notifies;
            await API.EditData(null, 0);
            IssetNotify.Visibility = Visibility.Hidden;
            NotifyCount.Visibility = Visibility.Hidden;

        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var notify = (Notify)(sender as System.Windows.Controls.StackPanel).DataContext;
            var NotifyPanel = sender as System.Windows.Controls.StackPanel;
            if (notify.IsSeen == 0)
            {
                NotifyPanel.Background = new SolidColorBrush(Color.FromRgb(106, 106, 106));
            }
        }

        private void NotifyList_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //NotifyPopup.Visibility = Visibility.Collapsed;
        }
    }

}
