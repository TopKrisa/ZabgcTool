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
    /// Логика взаимодействия для CommentEdit.xaml
    /// </summary>
    public partial class CommentEdit : Window
    {
        int Id;
        public CommentEdit(string name, string question,string answer, int id)
        {
            InitializeComponent();
            Name.Text = name;
            Question.Text = question;
            Answer.Text = answer;
            Id = id;
            BackBTN.Click += (s, e) => { Close(); };
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            await new APIKeys.Core.APIRequest<Comments>(APIKeys.Core.DataTableNames.Tables.Comment,APIKeys.Keys.Api.Admin).EditData(new Comments(Id, Question.Text, Answer.Text, Name.Text),Id);
            Close();
        }
    }
}
