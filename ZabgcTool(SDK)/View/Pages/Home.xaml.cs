using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZabgcTool_SDK_.Model.Data;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
       private APIKeys.Core.APIRequest<Main> API = new APIKeys.Core.APIRequest<Main>(APIKeys.Core.DataTableNames.Tables.Main, APIKeys.Keys.Api.Admin);
        APIKeys.Core.APIRequest<MetricaInfo> APIRequest = new APIKeys.Core.APIRequest<MetricaInfo>
                (APIKeys.Core.DataTableNames.Tables.Metrics,
                APIKeys.Keys.Api.Admin);
        private Window _Owner;
        public Home(Window Owner)
        {
            _Owner = Owner;
            InitializeComponent();
            GetMainInfo();
            GetMetricaInfo();

        }
        private async void GetMainInfo()
        {
            try
            {
                List<Main> main = await API.GetData();
                Materials.Content = main[0].Materials;
                Users.Content = main[0].Users;
                Pages.Content = main[0].Pages;
                Photo.Content = main[0].PhotoAlbums;
                Login.Content = new Helper.Settings().ReadSettings().Name;
    
                LoadingAnim1.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch
            {

            }
        }
        private async void GetMetricaInfo()
        {
            try
            {
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(
                 $"https://zabgc.zabedu.ru/api/metrics/=68c7a3cfe1b86a85bad1b580b91402c3");
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                Stream stream = response.GetResponseStream();

                string json = new StreamReader(stream).ReadToEnd();

                MetricaInfo Metrica = JsonConvert.DeserializeObject<MetricaInfo>(json);
                DateTime dateTime = new DateTime().AddSeconds(Metrica.AvgDuration);
                Clients.Content = string.Format("{0:d}", Metrica.CountUser);
                Denided.Content = string.Format("{0:f2} %", Metrica.PercentDenided);
                PageDepth.Content = string.Format("{0:f2}", Metrica.PageDepth);
                TimeOnSite.Content = string.Format("{0} минут", $"{dateTime.ToLongTimeString()}");
                LoadingAnim2.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch
            {

            }
        }

        private void ScheduleUpload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var Settings = new Helper.Settings().ReadSettings();
            List<string> Paths = new List<string>();
            
              if (string.IsNullOrEmpty(Settings.ShedulePath))
            {
                if (!Helper.Helper.IsWindowOpen<Settings>())
                {
                    new Settings(_Owner).Show();
                }
                return;
            }

            string[] files = Directory.GetFiles(Settings.ShedulePath);
            try
            {
                foreach (string obj in files)
                {
                    if (Directory.Exists(obj))
                    {
                        Paths.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                    }
                    else
                    {

                        Paths.Add(obj);
                    }
                }
                new FTP.MainFTP(_Owner).FtpUploader(Paths, "ra", Settings.Addres);
                new Notify().AddNotify("Выгрузка расписания завершена");
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                  .AddArgument("conversationId", 9813)
                           .AddText($"Выгрузка завершенна")
                    .AddText($"Расписание 2.0")
                    .Show();
            }
            catch
            {

            }
            finally{
                //DirectoryInfo dirInfo = new DirectoryInfo(Settings.ShedulePath);
                //foreach (FileInfo file in dirInfo.GetFiles())
                //{
                //    file.Delete();
                //}
            }
          
        }
    }
}
