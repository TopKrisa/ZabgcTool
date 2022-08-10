using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ZabgcTool_SDK_.APIKeys.Core;
using ZabgcTool_SDK_.Model;

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
                try
                {
                    List<Comments> comments = new List<Comments>();
                    await Task.Run(async () =>
                    {
                        comments = await API.GetData();
                    });
                    CommentsList.DataContext = comments;
                }
                finally
                {
                    LoadAnim.Visibility = Visibility.Collapsed;
                }
            };


        }

        private async void PublishComment_Click(object sender, RoutedEventArgs e)
        {
            Comments comments = (Comments)(sender as Button).DataContext;
            await new APIKeys.Core.APIRequest<Comments>
                (APIKeys.Core.DataTableNames.Tables.Comment, APIKeys.Keys.Api.Admin)
                  .EditData(new Comments(comments.Id, comments.Message,
                     comments.Answer, comments.Name), comments.Id);
            CommentsList.DataContext = await API.GetData();
        }

        private async void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            List<Comments> comments = new List<Comments>();
            Comments comment = (Comments)(sender as Button).DataContext;
            await Task.Run(async () =>
            {
                await API.DeleteData(comment.Id);
                comments = await API.GetData();
            });
            CommentsList.DataContext = comments;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Comments comments = (Comments)(sender as Grid).DataContext;
            var comment = sender as Grid;
            if (comments.Activate == 0)
            {
                comment.Background = Brushes.DarkRed;
            }

        }

        private async void DeleteCoupleQuestions_Click(object sender, RoutedEventArgs e)
        {
            List<Comments> comments = new List<Comments>();
            foreach (int item in Selected)
            {
                await Task.Run(async () =>
                {
                    await API.DeleteData(item);

                });
            }
            Selected.Clear();
            await Task.Run(async () =>
            {
                comments = await API.GetData();
            });
            CommentsList.DataContext = comments;
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

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
