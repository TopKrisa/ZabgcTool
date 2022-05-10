using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Magazin.xaml
    /// </summary>
    public partial class Magazin : Page
    {
        private Model.Data.Newspapper Newspapper;
        public Magazin()
        {
            InitializeComponent();
            LoadData(Newspaper);
           
        }
        private async void LoadData(System.Windows.Controls.ListView listView)
        {
            var list = await new APIKeys.Core.APIRequest<Model.Data.Newspapper>(APIKeys.Core.DataTableNames.Tables.Newspaper, APIKeys.Keys.Api.Admin).GetData();

            if (list.Count == 0)
            {
                NoMagazinData.Visibility = Visibility.Visible;
            }
            else
            {
                NoMagazinData.Visibility = Visibility.Collapsed;
                listView.DataContext = list;
            }
        }

        private void Opens_Click(object sender, RoutedEventArgs e)
        {
            var Newspaper = (Model.Data.Newspapper)(sender as System.Windows.Controls.Button).DataContext;
            System.Diagnostics.Process.Start($"https://zabgc.zabedu.ru/{Newspaper.Url}");
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Helper.Settings().ReadSettings();
            var Newspaper = (Model.Data.Newspapper)(sender as System.Windows.Controls.Button).DataContext;
            await new FTP.Client(settings.Addres, settings.Login, settings.Password).DeleteFile(Newspaper.Url);
            await new APIKeys.Core.APIRequest<Model.Data.Newspapper>(APIKeys.Core.DataTableNames.Tables.Newspaper, APIKeys.Keys.Api.Admin).DeleteData(Newspaper.Id);
        }
        private long SizeFile;
        private string Url;
        string PathFile;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF | *.pdf;";
            var settings = new Helper.Settings().ReadSettings();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Url = $"{Environment.TickCount}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}{Path.GetExtension(openFileDialog.FileName)}";
                SizeFile = new FileInfo(openFileDialog.FileName).Length/ 1048576;
                PathFile = openFileDialog.FileName;
                AcceptBorder.Visibility = Visibility.Visible;
            }
           
        }

        private async void SaveNewsPapper(object sender, RoutedEventArgs e)
        {
            var settings = new Helper.Settings().ReadSettings();
            await new FTP.Client(settings.Addres, settings.Login, settings.Password).UploadFile(PathFile, Url);
            await new APIKeys.Core.APIRequest<Model.Data.Newspapper>(APIKeys.Core.DataTableNames.Tables.Newspaper, APIKeys.Keys.Api.Admin).AddData(new Model.Data.Newspapper() { Name = NameNewspaper.Text, Format = ".pdf",Size = SizeFile.ToString(),Url = Url});
            AcceptBorder.Visibility = Visibility.Collapsed;
          LoadData(Newspaper);
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            AcceptBorder.Visibility = Visibility.Collapsed;
        }
    }
}
