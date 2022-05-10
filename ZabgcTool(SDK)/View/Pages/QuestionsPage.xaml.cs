using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ZabgcTool_SDK_.APIKeys.Core;
using ZabgcTool_SDK_.Model;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для QuestionsPage.xaml
    /// </summary>
    public partial class QuestionsPage : Page
    {
        private APIRequest<Qusestions> API = new APIRequest<Qusestions>(DataTableNames.Tables.Questions, APIKeys.Keys.Api.Admin);
        private List<int> Selected = new List<int>();
        public QuestionsPage(TextBlock text)
        {
            InitializeComponent();
            text.Visibility = Visibility.Visible;
            text.Text = "Вопросы";
            Loaded += async (s, e) =>
                {
                    List<Qusestions> qusestions = await API.GetData();
                
                       QuestionList.DataContext = qusestions;
                   
                    
                }; 
            
        }

        private  void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var Qusestions = (Qusestions)(sender as Grid).DataContext;
            var quest = sender as Grid;
            if(Qusestions.Activity == 0)
            {
                quest.Background = Brushes.DarkRed;
            }
         
        }

        private async void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            var Qusestions = (Qusestions)(sender as Button).DataContext;
           string ret = await API.DeleteData(Qusestions.Id);
            QuestionList.DataContext = await API.GetData();
        }

        private async void PublishQuestion_Click(object sender, RoutedEventArgs e)
        {
            var Qusestions = (Qusestions)(sender as Button).DataContext;
            if (!Helper.Helper.IsWindowOpen<QuestionEdit>())
            {
               new QuestionEdit(Qusestions.Id, Qusestions.User, Qusestions.Question, QuestionList).Show();
            }
            QuestionList.ItemsSource = await API.GetData();
        }

        private async void DeleteCoupleQuestions_Click(object sender, RoutedEventArgs e)
        {
            foreach(int item in Selected)
            {
                string ret = await API.DeleteData(item);
            }
            Selected.Clear();
            QuestionList.DataContext= await API.GetData();
            DeleteCoupleQuestions.Visibility = Visibility.Collapsed;
        }

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as ToggleButton;
            DeleteCoupleQuestions.Visibility = Visibility.Visible;
               var SelectedQuestion = (Qusestions)(sender as ToggleButton).DataContext;
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
