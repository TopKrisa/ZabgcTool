using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News : Page
    {
        APIKeys.Core.APIRequest<Model.Data.News> API = new APIKeys.Core.APIRequest<Model.Data.News>(APIKeys.Core.DataTableNames.Tables.News, APIKeys.Keys.Api.Admin);
        public News()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                LoadedData();
            };
        }
        
        private async void LoadedData()
        {
            List<Model.Data.News> news = await API.GetData();
         
            if(news.Count == 0)
            {
                NoNewsData.Visibility = Visibility.Collapsed;
            }
            else
            {
            NewsList.DataContext = news;
                NoNewsData.Visibility = Visibility.Collapsed;
            }
        }
        private void OpenNews_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.News news = (Model.Data.News)(sender as System.Windows.Controls.Button).DataContext;

            System.Diagnostics.Process.Start($"http://zabgc.zabedu.ru/news.php?id={news.Id}");


        }
        private Model.Data.News New;
        private bool CheckAdd;
        private void EditNews_Click(object sender, RoutedEventArgs e)
        {
            New = (Model.Data.News)(sender as System.Windows.Controls.Button).DataContext;
            NameOfData.Text = New.Name;
            Descrition.Text = New.Description;
            var Editor = new HtmlRedactor.EditRedactor(New.Message);
            Editor.Show();
            Editor.Closing += async (s, es) =>
            {
                AcceptBorder.Visibility = Visibility.Visible;
                CheckAdd = false;
            };
        }

        private async void DeleteNews_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.News news = (Model.Data.News)(sender as System.Windows.Controls.Button).DataContext;
            await API.DeleteData(news.Id);
            LoadedData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Editor = new HtmlRedactor.EditRedactor();
            Editor.Show();
            Editor.Closing += async (s, es) =>
            {
                AcceptBorder.Visibility = Visibility.Visible;
                CheckAdd = true;
            };
        }

        private async void AccBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameOfData.Text) && !string.IsNullOrWhiteSpace(Descrition.Text))
            {
                if (!CheckAdd)
                {
                    if (string.IsNullOrWhiteSpace(PathPreview))
                    {
                        await new APIKeys.Core.APIRequest<Model.Data.News>(APIKeys.Core.DataTableNames.Tables.News, APIKeys.Keys.Api.Admin).EditData(new Model.Data.News() { Name = NameOfData.Text, Description = Descrition.Text, Poster = New.Poster, Message = HtmlRedactor.Saver.Text }, New.Id);

                    }
                    else
                    {
                        await new APIKeys.Core.APIRequest<Model.Data.News>(APIKeys.Core.DataTableNames.Tables.News, APIKeys.Keys.Api.Admin).EditData(new Model.Data.News() { Name = NameOfData.Text, Description = Descrition.Text, Poster = PathPreview, Message = HtmlRedactor.Saver.Text }, New.Id);

                    }
                    AcceptBorder.Visibility = Visibility.Collapsed;

                }
                else
                {
                    await new APIKeys.Core.APIRequest<Model.Data.News>(APIKeys.Core.DataTableNames.Tables.News, APIKeys.Keys.Api.Admin).AddData(new Model.Data.News() { Name = NameOfData.Text, Description = Descrition.Text, Poster = PathPreview, Message = HtmlRedactor.Saver.Text });
                    AcceptBorder.Visibility = Visibility.Collapsed;

                }
                LoadedData();
            }
        }
       
        private string PathPreview;
        private async void Preview_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images | *.png; *.jpg; *.jpeg; *gfif; ";
            var settings = new Helper.Settings().ReadSettings();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathPreview = @"/file/"+$"{Environment.TickCount}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}{Path.GetExtension(openFileDialog.FileName)}";
                await new FTP.Client(settings.Addres, settings.Login, settings.Password).UploadFile(openFileDialog.FileName, PathPreview);
                AcceptBorder.Visibility = Visibility.Visible;
            }
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            AcceptBorder.Visibility = Visibility.Collapsed;
        }
    }
}
