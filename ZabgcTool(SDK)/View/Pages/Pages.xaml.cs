using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
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
    /// Логика взаимодействия для Pages.xaml
    /// </summary>
    public partial class Pages : Page
    {
        public static string Text;
        public Pages()
        {
            InitializeComponent();
            Refresh();
        }
        private List<Model.Data.Pages> pages;
        private async void Refresh()
        {
            await Task.Run(() => {
                Dispatcher.Invoke(async () => {
                    pages = await new APIKeys.Core.APIRequest<Model.Data.Pages>(APIKeys.Core.DataTableNames.Tables.Pages, APIKeys.Keys.Api.Admin).GetData();
                    if (pages.Count == 0)
                    {
                        NoPageData.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lv.DataContext = pages.OrderBy(x => x.Id).Reverse();
                        NoPageData.Visibility = Visibility.Collapsed;
                    }
                    
                });
            });
    
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.Pages pages = (Model.Data.Pages)(sender as Button).DataContext;
            System.Diagnostics.Process.Start($"http://zabgc.zabedu.ru/page.php?id={pages.Id}");
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.Pages pages = (Model.Data.Pages)(sender as Button).DataContext;
            await new APIKeys.Core.APIRequest<Pages>(APIKeys.Core.DataTableNames.Tables.Pages, APIKeys.Keys.Api.Admin).DeleteData(pages.Id);
            Refresh();
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lv.DataContext = pages.Where(x =>x.Name.ToLower().Contains(Search.Text.ToLower()));
        
        }
        private Model.Data.Pages Page;
        private async  void Edit_Click(object sender, RoutedEventArgs e)
        {
            Page = (Model.Data.Pages)(sender as Button).DataContext;
            string text = Page.Text;
            NameOfData.Text = Page.Name;
            var Editor = new HtmlRedactor.EditRedactor(text);
            Editor.Show();
            Editor.Closing += async (s, es) =>
            {
                AcceptBorder.Visibility = Visibility.Visible;
                CheckAdd = false;
            };
        }
        bool CheckAdd;
        private async void AddPage_Click(object sender, RoutedEventArgs e)
        {
            
               
            var Editor = new HtmlRedactor.EditRedactor();
            Editor.Show();
            Editor.Closing += async (s, es) =>
            {
                AcceptBorder.Visibility = Visibility.Visible;
                CheckAdd = true;
            };
            
            

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameOfData.Text) )
            {
                if (!CheckAdd)
                {
                    await new APIKeys.Core.APIRequest<Model.Data.Pages>(APIKeys.Core.DataTableNames.Tables.Pages, APIKeys.Keys.Api.Admin).EditData(new Model.Data.Pages() { Id = Page.Id, Name = NameOfData.Text, Text = HtmlRedactor.Saver.Text }, Page.Id);
                    AcceptBorder.Visibility = Visibility.Collapsed;

                }
                else
                {
                    await new APIKeys.Core.APIRequest<Model.Data.Pages>(APIKeys.Core.DataTableNames.Tables.Pages, APIKeys.Keys.Api.Admin).AddData(new Model.Data.Pages() {Name = NameOfData.Text, Text = HtmlRedactor.Saver.Text});
                    AcceptBorder.Visibility = Visibility.Collapsed;

                }
                Refresh();
            }
        }

        private void CloseSaver_Click(object sender, RoutedEventArgs e)
        {
            AcceptBorder.Visibility = Visibility.Collapsed;
        }
    }
}
