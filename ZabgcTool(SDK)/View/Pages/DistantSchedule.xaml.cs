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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для DistantSchedule.xaml
    /// </summary>
    public partial class DistantSchedule : Page
    {
        public DistantSchedule()
        {
            InitializeComponent();
            LoadData(Schedule);
        }
        private async void LoadData(ListView listView)
        {
            listView.DataContext = await new APIKeys.Core.APIRequest<Model.Data.InternalSchedule>(APIKeys.Core.DataTableNames.Tables.Distant, APIKeys.Keys.Api.Admin).GetData();
        }
        string Url = string.Empty;
        string PathFile = string.Empty;
        private void UploadShedule_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "PDF|*.pdf;";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Url = $"file/{Environment.TickCount}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}{Path.GetExtension(openFileDialog.FileName)}";
                PathFile = openFileDialog.FileName;
                NameSchedule.Text = $"Расписание учебного процесса";
                AcceptBorder.Visibility = Visibility.Visible;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.InternalSchedule @internal = (Model.Data.InternalSchedule)(sender as Button).DataContext;
            System.Diagnostics.Process.Start($"https://zabgc.zabedu.ru/{@internal.Url}");
        }

        private  void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (!Helper.Helper.IsWindowOpen<DeleteDialog>())
            {
                        var Internal = (Model.Data.InternalSchedule)(sender as Button).DataContext;
                new DeleteDialog(async () => {
                    await Dispatcher.Invoke(async () =>
                    {
                        var settings = new Helper.Settings().ReadSettings();
                        //  await new FTP.Client(settings.Addres, settings.Login, settings.Password).DeleteFile(Internal.Url);
                        await new APIKeys.Core.APIRequest<Model.Data.InternalSchedule>(APIKeys.Core.DataTableNames.Tables.Distant, APIKeys.Keys.Api.Admin).DeleteData(Internal.Id);
                        LoadData(Schedule);
                    });
                   
                }).Show();
            }

            }
            finally
            {

            LoadData(Schedule);
            }
        }

        private async void SaveSchedule_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Helper.Settings().ReadSettings();
            await new FTP.Client(settings.Addres, settings.Login, settings.Password).UploadFile(PathFile, Url);
            await new APIKeys.Core.APIRequest<Model.Data.InternalSchedule>(APIKeys.Core.DataTableNames.Tables.Distant, APIKeys.Keys.Api.Admin).AddData(new Model.Data.InternalSchedule()
            {
                Name = NameSchedule.Text,
                Url = Url,
                Format = Path.GetExtension(PathFile),
                JsonSize = string.Format("{0:f1} kb", new FileInfo(PathFile).Length / 1024)
            });
            AcceptBorder.Visibility = Visibility.Collapsed;
            LoadData(Schedule);
        }
        private void butn_Click(object sender, RoutedEventArgs e)
        {
            AcceptBorder.Visibility = Visibility.Collapsed;
        }
    }
}
