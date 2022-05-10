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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public bool Load;
        public AdminPanel(TextBlock text, Window Owner)
        {
            InitializeComponent();
            text.Visibility = Visibility.Visible;
            
            text.Text = "Панель управления";
            WorkSpace.Navigate(new Home(Owner));
            Home.Focus();
            Home.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Home(Owner));
                text.Visibility = Visibility.Collapsed;
            };
            Gallery.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Gallery(WorkSpace));
                text.Visibility = Visibility.Collapsed;
            };
            Departments.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Departments());
                text.Visibility = Visibility.Collapsed;
            };
            Schedule.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Schedule(WorkSpace));
                text.Visibility = Visibility.Collapsed;
            };
            Magazin.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Magazin());
                text.Visibility = Visibility.Collapsed;
            };
            Pages.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Pages());
                text.Visibility = Visibility.Collapsed;
            };
            News.Click += (s, e) =>
            {
                WorkSpace.Navigate(new News());
                text.Visibility = Visibility.Collapsed;
            };
        }
    }
}
