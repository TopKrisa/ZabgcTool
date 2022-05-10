using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ZabgcTool_SDK_.View;
using ZabgcTool_SDK_.Model;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;

namespace ZabgcTool_SDK_.FTP
{
    /// <summary>
    /// Логика взаимодействия для MainFTP.xaml
    /// </summary>
    public partial class MainFTP : Page
    {
        private ZabgcTool_SDK_.Helper.Settings Settings = new ZabgcTool_SDK_.Helper.Settings().ReadSettings();
        
        private string prevAdress = "ftp://";

        private string Addres;

        private string Login;

        private string Password;

        private int bufferSize = 1024;

        private string FileName;

        public bool Passive = true;

        public bool Binary = true;

        public bool EnableSsl = false;
        Window MainWindow;

        public bool Hash = false;
        private string DirName;
        private string DirAddress;
        
        
        public MainFTP(Window Owner)
        {
            InitializeComponent();
            MainWindow = Owner;
           
            if (string.IsNullOrEmpty(Settings.Addres) || string.IsNullOrEmpty(Settings.Password) || string.IsNullOrEmpty(Settings.Login))
            {
                NoFTPData.Visibility = Visibility.Visible;
                LoadAnim.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoFTPData.Visibility = Visibility.Collapsed;
                Addres = Settings.Addres;
                Login = Settings.Login;
                Password = Settings.Password;
               
                Message($"Попытка подключения к {Addres}");
                string Hostname = Settings.Addres;
                var SplitedName = Hostname.Split('/');
                Hostname = SplitedName.FirstOrDefault(x => x.Contains(".ru"));
                if (Helper.Helper.CheckInternet(Hostname))
                {
                    ASFTP();
                }
                else
                {
                    lbx_files.Visibility = Visibility.Collapsed;
                    NotConnectedPopUp.Visibility = Visibility.Visible;
                    LoadAnim.Visibility = Visibility.Collapsed;
                    Message("Удаленный сервер не отвечает");
                }
                Message($"Подключенно к {Addres}");
            }
            GotoSettings.Click += (s, e) =>
            {
                if (!Helper.Helper.IsWindowOpen<Settings>())
                {
                    new Settings(Owner).Show();
                }
 
            };
        }
       private void Message(string message)
        {
           CommandsList.Items.Add($"<{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}>: {message}");
            var Scroll = FindScrollViewer(CommandsList);
            if(Scroll != null)
            {
                Scroll.ScrollChanged += (s, e) =>
                {
                    if (e.ExtentHeightChange > 0)
                        Scroll.ScrollToBottom();
                };
            }
         
        }
        private static ScrollViewer FindScrollViewer(DependencyObject root)
        {
            var queue = new Queue<DependencyObject>(new[] { root });

            do
            {
                var item = queue.Dequeue();

                if (item is ScrollViewer)
                    return (ScrollViewer)item;

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
                    queue.Enqueue(VisualTreeHelper.GetChild(item, i));
            } while (queue.Count > 0);

            return null;
        }
        private FtpWebRequest createRequest(string uri, string method)
        {
            var Request = (FtpWebRequest)WebRequest.Create(uri);
            Request.Credentials = new NetworkCredential(Login, Password);
            Request.Method = method;
            Request.UseBinary = Binary;
            Request.EnableSsl = EnableSsl;
            Request.UsePassive = Passive;
            return Request;
        }
        async void RemoveDirectoryWithFiles(string DirectoryName)
        {   string Directory = DirectoryName+@"/";
            try
            {
                LoadAnim.Visibility = Visibility.Visible;
                var RawFTPdata = new List<string>();
               
                var request = createRequest(Directory, WebRequestMethods.Ftp.ListDirectoryDetails);

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
                List<FileDirecoryInfo> Items = RawFTPdata
                            .Select(s =>
                            {
                                Match match = regex.Match(s);
                                if (match.Length > 5)
                                {
                                // Устанавливаем тип, чтобы отличить файл от папки (используется также для установки рисунка)
                                string type = match.Groups[1].Value == "d" ? "DIR.png" : $"{Path.GetExtension(match.Groups[6].Value)}.png"; ;

                                // Размер задаем тольк typeо для файлов, т.к. для папок возвращается
                                // размер ярлыка 4кб, а не самой папки
                                string size = "";
                                    if (type == $"{Path.GetExtension(match.Groups[6].Value)}.png")
                                        size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                    return new FileDirecoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, Addres);
                                }
                                else return new FileDirecoryInfo();
                            }).OrderBy(x => x.Type == "DIR.png").ToList();
                Items = Items.Where(x => x.Name != "..").ToList();
                Items = Items.Where(x => x.Name != ".").ToList();
                foreach (var item in Items)
                {

                    if (item.Type != "DIR.png" && item.Name != "Назад")
                    {
                        RemoveFile(DirAddress,$"{Directory}{item.Name}");
                    }
                    else if (item.Type == "DIR.png" && item.Type != "Назад")
                    {

                        RemoveDirectoryWithFiles(DirAddress + Directory + item.Name);
                        RemoveDirectory($"{DirAddress}", $"{Directory}{item.Name}");
                    }
                }
                RemoveDirectory(DirAddress,DirName);
                RemoveDirectory(DirAddress,Directory);
                LoadAnim.Visibility = Visibility.Collapsed;
                ASFTP();
            }
            catch(Exception ex)
            {
                Message($"{ex.Message} {DirAddress}{Directory} ");
                LoadAnim.Visibility = Visibility.Collapsed;
            }
       
        }
        async void ASFTP()
        {
           
            try
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    lbx_files.Visibility = Visibility.Collapsed;
                    NotConnectedPopUp.Visibility = Visibility.Visible;
                    LoadAnim.Visibility = Visibility.Collapsed;
                    Message("Отсутствует подключение к Интернету");
                }
                else
                {
                    NotConnectedPopUp.Visibility = Visibility.Collapsed;
                    lbx_files.Visibility = Visibility.Visible;
                    LoadAnim.Visibility = Visibility.Visible;
                    var RawFTPdata = new List<string>();

                    var request = createRequest(Addres, WebRequestMethods.Ftp.ListDirectoryDetails);

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
                    Items.Add(new FileDirecoryInfo("", "DEFAULT.png", "Назад", "", Addres));
                    Items.AddRange(RawFTPdata
                                .Select(s =>
                                {
                                    Match match = regex.Match(s);
                                    if (match.Length > 5)
                                    {
                                    // Устанавливаем тип, чтобы отличить файл от папки (используется также для установки рисунка)
                                    string type = match.Groups[1].Value == "d" ? "DIR.png" : $"{Path.GetExtension(match.Groups[6].Value)}.png"; ;

                                    // Размер задаем тольк typeо для файлов, т.к. для папок возвращается
                                    // размер ярлыка 4кб, а не самой папки
                                    string size = "";
                                        if (type == $"{Path.GetExtension(match.Groups[6].Value)}.png")
                                            size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                        return new FileDirecoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, Addres);
                                    }
                                    else return new FileDirecoryInfo();
                                }).OrderByDescending(x => x.Type == "DIR.png").ToList());

                    //// Добавить поле, которое будет возвращать пользователя на директорию выше

                   
                    Items = Items.Where(x => x.Name != "..").ToList();
                    Items = Items.Where(x => x.Name != ".").ToList();
                  //  Items.Reverse();
                    lbx_files.DataContext = Items;
                    LoadAnim.Visibility = Visibility.Collapsed;
                    TextUri.Text = Addres;
                    if (Addres == "ftp://zabedu.ru/web/" || Addres == "ftp://zabedu.ru/")
                    {
                        Message($"Вы в начале дерева {Addres}");
                    }
                    else
                    {
                        Message($"{Addres}");
                    }
                }
              
            }
            catch (Exception e)
            {
                Message(e.Message);
            }
        }
       
        private void folder_Click(object sender, MouseButtonEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;

            FileDirecoryInfo fdi = (FileDirecoryInfo)(sender as StackPanel).DataContext;
            TypeFile.Text = Path.GetExtension(fdi.Name);
            SizeFiles.Text = fdi.FileSize;
            if(String.IsNullOrEmpty(fdi.FileSize))
            {
                TypeFile.Text = "Папка";
                SizeFiles.Text = "Нет";
            }

            if (e.ClickCount >= 2)
            {
                if (fdi.Type == "DIR.png")
                {
                    prevAdress = fdi.adress;
                    Addres = fdi.adress + fdi.Name + "/";
                    ASFTP();
                }
                else if (fdi.Type == "DEFAULT.png")
                {
                    if (fdi.Name == "Назад")
                    {
                       
                            string addr = "";
                            string[] splitwords = fdi.adress.Split(new char[] { '/' });
                            for (int inc = 0; inc <= splitwords.Count() - 3; inc++)
                            {
                                addr += $"{splitwords[inc]}/";

                            }
                            Addres = addr;
                        if (Addres != "ftp://zabedu.ru/")
                        {
                            ASFTP();
                        }
                       
                    }

                }
                else
                {
                    FileName = fdi.Name;
                    
                }
            }

        }
        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            
            List<string> Paths = new List<string>();
            foreach (string obj in (string[])e.Data.GetData(DataFormats.FileDrop))
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
            FtpUploader(Paths,DirName,Addres);
            

        }
 public async void FtpUploader(string address,List<string> list)
        {
            
            if(list.Count == 1)
            Message($"Начало загрузки {list.Count} файлa.");
            else
             Message($"Начало загрузки {list.Count} файлов.");
            foreach (string source in list)
            {
                {////////////////////////////////////////////////////////////

                    var request = createRequest(address + Path.GetFileName(source), WebRequestMethods.Ftp.UploadFile);

                    using (var stream = await request.GetRequestStreamAsync())
                    {
                        using (var fileStream = System.IO.File.Open(source, FileMode.Open))
                        {
                           
                            int num;

                            byte[] buffer = new byte[bufferSize];

                            while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (Hash)
                                    Console.Write("#");

                                await stream.WriteAsync(buffer, 0, num);
                         
                            }
                        }
                        Message($"Файл {source} загружен.");
                    }
                }
               
            }
            ASFTP();
          
        }
       public async void FTPSaver(string list, string DirName)
        {
            try
            {
                //MessageBox.Show(DirName);
                    {////////////////////////////////////////////////////////////

                        var request = createRequest($@"{DirName}/{Path.GetFileName(list)}", WebRequestMethods.Ftp.UploadFile);

                        using (var stream = await request.GetRequestStreamAsync())
                        {
                            using (var fileStream = System.IO.File.Open(list, FileMode.Open))
                            {
                                int num;

                                byte[] buffer = new byte[bufferSize];

                                while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (Hash)
                                        Console.Write("#");

                                    await stream.WriteAsync(buffer, 0, num);

                                }
                            }
                            Message($"Файл {list} загружен.");
                        ResponseProgresBar.Visibility = Visibility.Hidden;
                        }
                    }

                
                Message($"{list} сохранен");
                new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
              .AddText($"Файл {list} успешно сохранен")
              .Show();
                ASFTP();
            }
            catch (Exception we)
            {
                MessageBox.Show($"{we.Message},{Addres},{DirName},{FileName}");

            }


        }
       public async void FtpUploader(List<string> list, string DirName, string Address)
        {
            try
            {
                if (list.Count == 1)
                    Message($"Начало загрузки {list.Count} файлa.");
                else
                    Message($"Начало загрузки {list.Count} файлов.");
                foreach (string source in list)
                {
                    {////////////////////////////////////////////////////////////

                        var request = createRequest($@"{Address}/{DirName}/{Path.GetFileName(source)}", WebRequestMethods.Ftp.UploadFile);

                        using (var stream = await request.GetRequestStreamAsync())
                        {
                            using (var fileStream = System.IO.File.Open(source, FileMode.Open))
                            {
                                int num;

                                byte[] buffer = new byte[bufferSize];

                                while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (Hash)
                                        Console.Write("#");

                                    await stream.WriteAsync(buffer, 0, num);
                                   
                                }
                            }
                            Message($"Файл {source} загружен.");
                        }
                    }

                }
                Message($"{list.Count} файлов загружено.");
                new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
              .AddText($"Файлов {list.Count} успешно загруженно ")
              .Show();
                ASFTP();
            }
            catch(Exception we)
            {
                MessageBox.Show($"{we.Message},{Addres},{DirName},{FileName}");

            }


        }
        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            MainWindow.Activate();

            FileDirecoryInfo fdi = (FileDirecoryInfo)(sender as StackPanel).DataContext;
            if (fdi.Type == "DIR.png")
            {
                AminDrag.IsOpen = false;
            }
            else
            {
                AminDrag.IsOpen = true;
            }
            if (fdi.Type == "DIR.png" && fdi.Name !="." && fdi.Name != "..")
            {
                DirName = fdi.Name;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void StackPanel_DragLeave(object sender, DragEventArgs e)
        {
        }
        
       private void CreateDirectory_Button(object sender, RoutedEventArgs e)
        {
              Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; var CreateW = new CreateDirecrory();
            CreateW.ShowDialog();
            if(CreateW.IsCreated == true) 
            {
                var request = createRequest(Addres+CreateW.NewName, WebRequestMethods.Ftp.MakeDirectory);
                using (var response = request.GetResponse() as FtpWebResponse)
                {
                    Message(response.StatusDescription);
                }
                ASFTP();
            }

        }
        private async void Open_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            DownloadForIDE($"{Addres}{FileName}", $@"{Directory.GetCurrentDirectory()}\files\{FileName}");
            await Task.Delay(1000);
            switch (Path.GetExtension(FileName))
            {
                case ".php":
                    new IDE.IDEWindow(FastColoredTextBoxNS.Language.PHP, $@"{Directory.GetCurrentDirectory()}\files\{FileName}", Addres).Show();
                    break;
                case ".html":
                    new IDE.IDEWindow(FastColoredTextBoxNS.Language.HTML, $@"{Directory.GetCurrentDirectory()}\files\{FileName}", Addres).Show();
                    break;
                case ".cs":
                    new IDE.IDEWindow(FastColoredTextBoxNS.Language.CSharp, $@"{Directory.GetCurrentDirectory()}\files\{FileName}", Addres).Show();
                    break;
                case ".js":
                    new IDE.IDEWindow(FastColoredTextBoxNS.Language.JS, $@"{Directory.GetCurrentDirectory()}\files\{FileName}", Addres).Show();
                    break;
                case ".json":
                    new IDE.IDEWindow(FastColoredTextBoxNS.Language.JSON, $@"{Directory.GetCurrentDirectory()}\files\{FileName}", Addres).Show();
                    break;
                default:
                    Process.Start($@"{Directory.GetCurrentDirectory()}\files\{FileName}");
                    break;
            }


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
          
        }

        private void Save_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            DownloadFile(FileName, $@"{Syroot.Windows.IO.KnownFolders.Downloads.Path}\{FileName}",Addres);

        }
        private void DeleteDirectory_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; RemoveDirectoryWithFiles(Addres+DirName);
            //RemoveDirectory(DirName);
           
           
        }
        private void RemoveDirectory(string DirectoryAddress,string DirectoryName)
        {
            var request = createRequest($@"{DirectoryAddress}{DirectoryName}/", WebRequestMethods.Ftp.RemoveDirectory);
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                string ResponseMessage = response.StatusDescription;
            }
            Message($"Папка {DirectoryName} удалена.");
            ASFTP();
        }
            private void Delete_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; RemoveFile(Addres,FileName);            
            ASFTP();
        }
        public async void RemoveFile(string Address,string FileName)
        {
            var Request = createRequest(Address + FileName, WebRequestMethods.Ftp.DeleteFile);
            using (var response = await Request.GetResponseAsync())
            {
              string ResponseMessage =  response.ContentLength.ToString();
            }
                Message($"Файл {FileName} удален.");
            new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
              .AddText($"Файл {FileName} успешно удален ")
              .Show();
            ASFTP();
        }
        public async void DownloadFile(string source, string dest,string Address)
        {
            var request = createRequest(Address + source, WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[bufferSize];

            using (var response = (FtpWebResponse)request.GetResponse())
            {

                using (var stream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(dest, FileMode.OpenOrCreate))
                    {
                        int readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (Hash)
                                Console.Write("#");

                          await  fs.WriteAsync(buffer, 0, readCount);

                            ResponseProgresBar.Maximum = Convert.ToDouble(fs.Length / 1024);
                            readCount = stream.Read(buffer, 0, bufferSize);
                            ResponseProgresBar.Value = Convert.ToDouble(fs.Position / 1024);
                        }
                    }
                }
                Message($"Файл скачан ({dest})");
                
            }

            new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
              .AddText($"Файл {FileName} успешно скачан ")
            .AddText($"Путь: {dest}")
              .Show();
        }
        private void Rename_Button(object se, RoutedEventArgs e)
       {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            var RenameW = new RenameWindow(FileName);
            RenameW.ShowDialog();

            if(RenameW.IsRenamed == true)
            {
                Message(Rename(FileName, RenameW.NewName,Addres));
            }
            ASFTP();
       }
        private void SaveAs_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Message($"{FileName} , {dialog.SelectedPath}");
               DownloadFiles(FileName, dialog.SelectedPath+@"/");
            }
        }
        private void StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; 
            try
            {
                FileDirecoryInfo fdi = (FileDirecoryInfo)(sender as StackPanel).DataContext;
                if (fdi.Type == "DEFAULT.png")
                {
                    if (fdi.Name == "Назад")
                    {
                        Popups.IsOpen = false;
                        PopupDIR.IsOpen = false;
                    }
                }
                else if(fdi.Type == "DIR.png")
                {
                    PopupDIR.IsOpen = true;
                    DirName = fdi.Name;
                    DirAddress = fdi.adress;
                }
                else 
                {
                    FileName = fdi.Name;
                    Popups.IsOpen = true;
                }
            }
            catch
            {
            }


        }

        private void OpenFolder_Button(object sender, RoutedEventArgs e)
        {
            PopupDIR.IsOpen = false;
            Addres = DirAddress + DirName + "/";
            ASFTP();
        }
        public string Rename(string CurrentName, string newName,string Address)
        {
            var request = createRequest(Address + Path.GetFileName(CurrentName), WebRequestMethods.Ftp.Rename);
            request.RenameTo = newName;
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }
        private void SaveFolder_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Папка для загрузки";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath +"\\"+ DirName + "\\";
                Directory.CreateDirectory(path);
                Message(folderBrowserDialog.SelectedPath);
                Download(DirAddress + DirName + "/", path);
             }

        }


        private void butn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)

        {
            FunctionGrid.Visibility = Visibility.Collapsed;
            FunctionGrid.Visibility = Visibility.Visible;
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            FunctionGrid.Visibility = Visibility.Visible;
        }

        private void Page_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PopupDIR.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; Popups.IsOpen = false;
        }

        private void AminDrag_DragLeave(object sender, DragEventArgs e)
        {
            AminDrag.IsOpen = false;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
        }
        private void MassDownload(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            DirAddress = @"";
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Папка для загрузки";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               Message(folderBrowserDialog.SelectedPath);
               Download(@"ftp://zabedu.ru/web/", folderBrowserDialog.SelectedPath+@"\");
            }
            new ToastContentBuilder()
                 .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
               .AddText($"Все файлы успешно скачаны ")
               .AddText($"Путь: {folderBrowserDialog.SelectedPath + @"\"}")
            .Show();

        }
    
        async void Download(string DirName, string destenation)
        {

            try
            {
                     //DirAddress += $"{DirName}/";
                    
                    LoadAnim.Visibility = Visibility.Visible;
                Message(DirAddress+DirName);
                    var RawFTPdata = new List<string>();

                    var request = createRequest($"{DirName}", WebRequestMethods.Ftp.ListDirectoryDetails);
                     request.Timeout = 6000000;
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
                    List<FileDirecoryInfo> Items = RawFTPdata
                                .Select(s =>
                                {
                                    Match match = regex.Match(s);
                                    if (match.Length > 5)
                                    {
                                    // Устанавливаем тип, чтобы отличить файл от папки (используется также для установки рисунка)
                                    string type = match.Groups[1].Value == "d" ? "DIR.png" : $"{Path.GetExtension(match.Groups[6].Value)}.png"; ;

                                    // Размер задаем тольк typeо для файлов, т.к. для папок возвращается
                                    // размер ярлыка 4кб, а не самой папки
                                    string size = "";
                                        if (type == $"{Path.GetExtension(match.Groups[6].Value)}.png")
                                            size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                        return new FileDirecoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, Addres);
                                    }
                                    else return new FileDirecoryInfo();
                                }).OrderBy(x => x.Type == "DIR.png").ToList();
                    Items = Items.Where(x => x.Name != "..").ToList();
                    Items = Items.Where(x => x.Name != ".").ToList();
                    Items = Items.Where(x => x.Name != "Назад").ToList();
                   Items = Items.Where(x => x.Name != "file").ToList();
                   Items = Items.Where(x => x.Name != "avatar").ToList();
                   Items = Items.Where(x => x.Name != "tphoto").ToList();
                   Items = Items.Where(x => x.Name != "images").ToList();
                   Items = Items.Where(x => x.Name != "metod").ToList();
                   Items = Items.Where(x => x.Name != "ra").ToList();
                    string PrivateDest;
                    Message(Items.Count.ToString());
                List<string> Paths = new List<string>();
                foreach (var item in Items)
                {

                    if (item.Type != "DIR.png" && item.Name != "Назад")
                    {
                        await Task.Run(() =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                DownloadFiles(DirName + item.Name, destenation + item.Name);
                            });
                        });
                        await Task.Delay(100);
                    }
                    else
                    if (item.Type == "DIR.png" && item.Name != "Назад")
                    {
                        PrivateDest = destenation + item.Name + @"\";
                        Paths.Add(PrivateDest);
                        if (!Directory.Exists(PrivateDest))
                        {
                            Directory.CreateDirectory(PrivateDest);
                            new Thread(()=> {
                                Dispatcher.Invoke(() =>
                                {
                                    Download($"{item.adress}{item.Name}/", PrivateDest);
                                });}).Start();
                                    
                        }
                       
                    }
                }
               
                Message($"Скачивание в папку {destenation} законченно");
                LoadAnim.Visibility = Visibility.Collapsed;
                ResponseProgresBar.Value = 0;
               
            }
                catch (Exception ex)
                {
                    Message(ex.Message);
                 }
            finally{
                new ToastContentBuilder()
             .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
           .AddText($"Все файлы успешно скачаны ")
           .AddText($"Путь: {destenation}")
        .Show();
                LoadAnim.Visibility = Visibility.Collapsed;
            }
}
        public async void DownloadFiles(string source, string dest)
        {
            var request = createRequest(source, WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[bufferSize];

            using (var response = (FtpWebResponse)request.GetResponse())
            {

                using (var stream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(dest, FileMode.OpenOrCreate))
                    {
                        int readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (Hash)
                                Console.Write("#");

                            await fs.WriteAsync(buffer, 0, readCount);

                            ResponseProgresBar.Maximum = Convert.ToDouble(fs.Length / 1024);
                            readCount = stream.Read(buffer, 0, bufferSize);
                            ResponseProgresBar.Value = Convert.ToDouble(fs.Position / 1024);
                        }
                    }
                }
                Message($"Файл скачан ({dest}/)");

            }

        }
        public async void DownloadForIDE(string source, string dest)
        {
            var request = createRequest(source, WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[bufferSize];

            using (var response = (FtpWebResponse)request.GetResponse())
            {

                using (var stream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(dest, FileMode.OpenOrCreate,FileAccess.ReadWrite))
                    {
                        int readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (Hash)
                                Console.Write("#");

                            await fs.WriteAsync(buffer, 0, readCount);
                            readCount = stream.Read(buffer, 0, bufferSize);
                            
                        }
                        
                    }
                    
                }
                Message($"Файл скачан ({dest}/)");
                
                
            }
            
        }

        private void RenameDIR_Button(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; var RenameW = new RenameWindow(DirName);
            RenameW.ShowDialog();

            if (RenameW.IsRenamed == true)
            {
                Message(Rename(DirName, RenameW.NewName, Addres));
            }
            ASFTP();
        }

        private void AminDrag_Drop(object sender, DragEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed; List<string> Paths = new List<string>();
            foreach (string obj in (string[])e.Data.GetData(DataFormats.FileDrop))
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
            FtpUploader(Addres,Paths);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Popups.IsOpen = false;
            PopupDIR.IsOpen = false;
            AminDrag.IsOpen = false;
            FunctionGrid.Visibility = Visibility.Collapsed;
            List<string> Paths = new List<string>();
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
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
                FtpUploader(Paths, @"ra", Addres);
                new Notify().AddNotify("Выгрузка расписания завершена");
                new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
              .AddText($"Выгрузка завершенна")
            .AddText($"Путь: {Addres}/ra")
              .Show();
            }
        }

    
        private void CreateBackup_Button(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(
             $"http://zabgc.ru/api/backup/={new APIKeys.Keys(APIKeys.Keys.Api.Admin).Key}");
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            string json = new StreamReader(stream).ReadToEnd();

            Message(json);
        }
    }
}
