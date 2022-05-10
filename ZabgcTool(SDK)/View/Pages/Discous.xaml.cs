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
    /// Логика взаимодействия для Discous.xaml
    /// </summary>
    public partial class Discous : Page
    {
        public Discous(Frame frame, TextBlock text)
        {
            InitializeComponent();
            text.Visibility = Visibility.Collapsed;
            Question.Click += (s, e) => 
            {
                frame.Navigate(new QuestionsPage(text));
            };
            Comment.Click += (s, e) =>
            {
                frame.Navigate(new CommPage(text));
            };
        }
    }
}
