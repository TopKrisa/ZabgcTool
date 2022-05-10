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
    /// Логика взаимодействия для QuestionEdit.xaml
    /// </summary>
    public partial class QuestionEdit : Window
    {
        private int ID;
        private string User;
        private string Quest;
        private ListView ListView;
        public QuestionEdit(int id, string user, string question, ListView listView)
        {
            InitializeComponent();
            ID = id;
            ListView = listView;
            Name.Text = user;
            User = user;
            Question.Text = question;
            Quest = question;
            BackBTN.Click += (s, e) =>  Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void RenameButton_Click(object sender, RoutedEventArgs e)
        {

            await new APIKeys.Core.APIRequest<Qusestions>(APIKeys.Core.DataTableNames.Tables.Questions, APIKeys.Keys.Api.Admin).EditData(
                new Qusestions() {Question = Quest,User= User,Activity= 1,Answer= Answer.Text,  Date= "", AnswerDate = "0",Id= ID, adminname = new Helper.Settings().AdminName}, ID
                );
            ListView.DataContext = await new APIKeys.Core.APIRequest<Qusestions>(APIKeys.Core.DataTableNames.Tables.Questions, APIKeys.Keys.Api.Admin).GetData();
            await Task.Delay(150);
            this.Close();
        }
    }
}
