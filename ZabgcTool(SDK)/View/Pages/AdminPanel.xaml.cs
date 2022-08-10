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
using ZabgcTool_SDK_.Model;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public bool Load;
        public AdminPanel(TextBlock text, Window Owner, int _Type)
        {
            InitializeComponent();

            switch (_Type)
            {
                case (int)Authorization.UserTypes.Admin:
                    break;
                case (int)Authorization.UserTypes.ScheduleAdmin:
                    Gallery.Visibility = Visibility.Collapsed;
                    Departments.Visibility = Visibility.Collapsed;
                    Magazin.Visibility = Visibility.Collapsed; 
                    Pages.Visibility = Visibility.Collapsed;
                    News.Visibility = Visibility.Collapsed; 

                    break;

            }

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
                text.Text = Gallery.Content.ToString();
            };
            Departments.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Departments());
                text.Text = Departments.Content.ToString();
            };
            Schedule.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Schedule(WorkSpace));
                text.Text = Schedule.Content.ToString();
            };
            Magazin.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Magazin());
                text.Text = Magazin.Content.ToString();
            };
            Pages.Click += (s, e) =>
            {
                WorkSpace.Navigate(new Pages());
                text.Text = Pages.Content.ToString();
            };
            News.Click += (s, e) =>
            {
                WorkSpace.Navigate(new News());
                text.Text = News.Content.ToString();
            };
        }
    }
}
