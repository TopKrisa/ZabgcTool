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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZabgcTool_SDK_.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : Page
    {
        private Frame Frame;
        public Gallery(Frame frame)
        {
            Frame = frame;
            InitializeComponent();
            Loaded += async (s, e) => 
            {

                List<Model.Data.Gallery> gallery = await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).GetData();
                GalleryList.DataContext = gallery;
            };
            CreateNewAlbum.Click += (s, e) =>
            {
                AcceptBorder.Visibility = Visibility.Visible;
              
            };
            AccBtn.Click += async (s, e) =>
            {
                List<Model.Data.Gallery> list = new List<Model.Data.Gallery>();
                List<Model.Data.Photo> Photos = new List<Model.Data.Photo>();
                int id = 0;
                try
                {
                    if (!string.IsNullOrWhiteSpace(NameOfData.Text))
                    {
                        await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).AddData(new Model.Data.Gallery()
                        {
                            Name = NameOfData.Text,
                            Count = "123",
                            Category = "123",
                            Photos = new List<Model.Data.Photo>(),
                            Id = 0
                        });
                        list = await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).GetData();
                         id = list.Max(x => x.Id);
                       Photos    = list.FirstOrDefault(x => x.Id == id).Photos;
                    }
                }
                finally
                {
               
                    Frame.Navigate(new GalleryPhotos(Photos, NameOfData.Text, Frame, id));
                }
                

            };
        }

        private void PublishGallery_Click(object sender, RoutedEventArgs e)
        {
            var gallery = (Model.Data.Gallery)(sender as Button).DataContext;
            Frame.Navigate(new GalleryPhotos(gallery.Photos,gallery.Name ,Frame, gallery.Id));

        }

        private void OpenGellery_Click(object sender, RoutedEventArgs e)
        {
            var gallery = (Model.Data.Gallery)(sender as Button).DataContext;

            System.Diagnostics.Process.Start($"http://zabgc.zabedu.ru/photo/{gallery.Id}-0");
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            AcceptBorder.Visibility = Visibility.Collapsed;
        }

        private  void DeleteGallery_Click(object sender, RoutedEventArgs e)
        {
            if (!Helper.Helper.IsWindowOpen<DeleteDialog>())
            {
                new DeleteDialog(async () => {
                    var gallery = (Model.Data.Gallery)(sender as Button).DataContext;
                    await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).DeleteData(gallery.Id);
                    GalleryList.DataContext = await new APIKeys.Core.APIRequest<Model.Data.Gallery>(APIKeys.Core.DataTableNames.Tables.Gallery, APIKeys.Keys.Api.Admin).GetData();
                }).Show();
            }
           
        }
    }
}
