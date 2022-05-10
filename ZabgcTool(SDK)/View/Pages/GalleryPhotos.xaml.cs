using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;


namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для GalleryPhotos.xaml
    /// </summary>
    public partial class GalleryPhotos : Page
    {
        APIKeys.Core.APIRequest<Model.Data.Photo> API = new APIKeys.Core.APIRequest<Model.Data.Photo>(APIKeys.Core.DataTableNames.Tables.Photos, APIKeys.Keys.Api.Admin);
        private const string Addres = "tphoto/preview/";
        private Model.Data.Photo Photo = default;
        private List<Model.Data.Photo> Photos;
        private int Id_album;
        private Frame Frame;
        public GalleryPhotos(List<Model.Data.Photo> photo, string Name, Frame frame, int id)
        {

            InitializeComponent();
            Logins.Text = Name;
            Id_album = id;
            Photos = photo;
            Frame = frame;
            Refresh(lv, Photos);
        }
        private void Refresh(System.Windows.Controls.ListView listView, List<Model.Data.Photo> list)
        {
            listView.DataContext = list;
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Functions.IsOpen = false;
            Photo = (Model.Data.Photo)(sender as StackPanel).DataContext;
            Functions.IsOpen = true;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Functions.IsOpen = false;
            OpenImage(Photo.Original);
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Functions.IsOpen = false;
            var settings = new Helper.Settings().ReadSettings();
            var Response = await API.DeleteData(Photo.Id);
            await new FTP.Client(settings.Addres, settings.Login, settings.Password).DeleteFile(Photo.Original);
        }
        private async void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            Functions.IsOpen = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images |*.jpg;*.jpeg;*.png; ";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog.FileName;
                string[] path = file.Split('\\');
                string newPath = string.Empty;
                string oldpath = path[path.Length - 1];
                for (int i = 0; i <= path.Length - 2; i++)
                {
                    newPath += path[i] + "\\";
                }
                newPath = newPath + $"0-{Environment.TickCount / 451}-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}{Path.GetExtension(oldpath)}";
                File.Move(file, newPath);
                var settings = new Helper.Settings().ReadSettings();

                await new FTP.Client(settings.Addres, settings.Login, settings.Password).UploadFile(newPath, Addres + Path.GetFileName(newPath));
                System.Windows.MessageBox.Show(Id_album.ToString());
                var Response = await API.AddData(new Model.Data.Photo() { Id_Album = Id_album, Date = DateTime.Now.ToShortDateString(), Original = $"tphoto/preview/{Path.GetExtension(newPath)}", Format = Path.GetExtension(newPath) });
               
            }
            var Photo = await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).GetData();
            Refresh(lv, Photo.FirstOrDefault(x => x.Id == Id_album).Photos);
        }
        private void OpenImage(string Uri)
        {
            System.Diagnostics.Process.Start(Uri);
        }

        private async void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            Functions.IsOpen = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog.FileName;
                string[] path = file.Split('\\');
                string newPath = string.Empty;
                string oldpath = path[path.Length - 1];
                for (int i = 0; i <= path.Length - 2; i++)
                {
                    newPath += path[i] + "\\";
                }
                newPath = newPath + $"0-{Environment.TickCount / 451}-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}{Path.GetExtension(oldpath)}";
                File.Move(file, newPath);
                var settings = new Helper.Settings().ReadSettings();

                await new FTP.Client(settings.Addres, settings.Login, settings.Password).UploadFile(newPath, Addres + Path.GetFileName(newPath));
                //System.Windows.MessageBox.Show($"tphoto/preview/{Path.GetFileName(newPath)}");
                var Response = await API.AddData(new Model.Data.Photo() { Id_Album = Id_album, Date = DateTime.Now.ToShortDateString(), Original = $"/tphoto/preview/{Path.GetFileName(newPath)}", Format = Path.GetExtension(newPath) });
            }
            var Photo = await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).GetData();
            Refresh(lv, Photo.FirstOrDefault(x => x.Id == Id_album).Photos);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            APIKeys.Core.APIRequest<Model.Data.Gallery> aPI = new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin);

            var Response = aPI.EditData(new Model.Data.Gallery() { Id = Id_album, Name = Logins.Text, Count = "0", Category = "0", Photos = Photos }, Id_album);

            Frame.Navigate(new Gallery(Frame));
        }
    }
}
