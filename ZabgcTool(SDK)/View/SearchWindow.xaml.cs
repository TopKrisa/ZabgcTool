using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZabgcTool_SDK_.FTP;

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private ListView ListView;
        private string Addres;
        public SearchWindow(ListView listView, string Address)
        {
            Addres = Address;
            ListView = listView;
            InitializeComponent();
            MouseLeftButtonDown += (s, e) =>
            {
                DragMove();
            };
            Yes.Click += async (s, e) => {
                listView.DataContext = await Search(SearchTextBox.Text, Address);
                Close();
            };
            No.Click += (s, e) =>
            {
                Close();
            };
            butn.Click += (s, e) =>
            {
                Close();
            };
           
         
       
        }

        private async void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ListView.DataContext = await Search(SearchTextBox.Text, Addres);
                Close();
            }
        }
        private async Task<List<FileDirecoryInfo>> Search(string SearchedText, string Addres)
        {
            try
            {
                var RawFTPdata = new List<string>();
                var request = new MainFTP(null).createRequest(Addres, WebRequestMethods.Ftp.ListDirectoryDetails);
                using (FtpWebResponse response = await request.GetResponseAsync() as FtpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, true))
                        {
                            while (!reader.EndOfStream)
                            {
                                RawFTPdata.Add(await reader.ReadLineAsync());
                            }
                        }
                    }
                }
                //ResponseProgresBar.Value = 0;
                RawFTPdata.ToArray();
                Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                List<FileDirecoryInfo> Items = new List<FileDirecoryInfo>();
                Items.Add(new FileDirecoryInfo("", "Icons/DEFAULT.png", "Назад", "", Addres));
                Items.AddRange(RawFTPdata
                            .Select(s =>
                            {
                                Match match = regex.Match(s);
                                if (match.Length > 5)
                                {
                                        // Устанавливаем тип, чтобы отличить файл от папки (используется также для установки рисунка)
                                        string type = match.Groups[1].Value == "d" ? "Icons/DIR.png" : $"Icons/{System.IO.Path.GetExtension(match.Groups[6].Value)}.png"; ;

                                        // Размер задаем тольк typeо для файлов, т.к. для папок возвращается
                                        // размер ярлыка 4кб, а не самой папки
                                        string size = "";
                                    if (type == $"{System.IO.Path.GetExtension(match.Groups[6].Value)}.png")
                                        size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                    return new FileDirecoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, Addres);
                                }
                                else return new FileDirecoryInfo();
                            }).OrderByDescending(x => x.Type == "Icons/DIR.png").ToList());

                Items = Items.Where(x => x.Name != "..").ToList();
                Items = Items.Where(x => x.Name != ".").ToList();
                Items = Items.Where(x => x.Name.ToLower().Contains(SearchedText)).ToList();
                return Items;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
