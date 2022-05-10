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
using ZabgcTool_SDK_.APIKeys.Core;
using System.Windows.Controls.Primitives;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для CommPage.xaml
    /// </summary>
    public partial class CommPage : Page
    {
        private APIRequest<Comments> API = new APIRequest<Comments>(DataTableNames.Tables.Comment, APIKeys.Keys.Api.Admin);
        private List<int> Selected = new List<int>();
        public CommPage(TextBlock text)
        { 

            InitializeComponent();
            text.Visibility = Visibility.Visible;
            text.Text = "Комментарии";
            Loaded += async (s, e) => 
            {
                CommentsList.DataContext = await API.GetData();
            };
            
            
        }

        private async void PublishComment_Click(object sender, RoutedEventArgs e)
        {
            Comments comments = (Comments)(sender as Button).DataContext;

            var WindowComments = new CommentEdit(comments.Name, comments.Message, comments.Otvet, comments.Id);
            if(!WindowComments.IsActive)
            {
                WindowComments.ShowDialog();
            }
            CommentsList.DataContext = await API.GetData();
        }

        private async void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            Comments comments = (Comments)(sender as Button).DataContext;

            await API.DeleteData(comments.Id);
            CommentsList.DataContext = await API.GetData();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Comments comments = (Comments)(sender as Grid).DataContext;
            var comment = sender as Grid;
            if(comments.Activate == 0)
            {
                comment.Background = Brushes.DarkRed;
            }

        }

        private async void DeleteCoupleQuestions_Click(object sender, RoutedEventArgs e)
        {
            foreach (int item in Selected)
            {
                string ret = await API.DeleteData(item);
            }
            Selected.Clear();
            CommentsList.DataContext = await API.GetData();
            DeleteCoupleQuestions.Visibility = Visibility.Collapsed;

        }

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as ToggleButton;
            DeleteCoupleQuestions.Visibility = Visibility.Visible;
            var SelectedQuestion = (Comments)(sender as ToggleButton).DataContext;
            if (Button.IsChecked.Value)
            {
                Selected.Add(SelectedQuestion.Id);
            }
            else
            {
                Selected = Selected.Where(x => x != SelectedQuestion.Id).ToList();
            }
            if (Selected.Count >= 1)
            {
                DeleteCoupleQuestions.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteCoupleQuestions.Visibility = Visibility.Collapsed;
            }
        }
    }
}
