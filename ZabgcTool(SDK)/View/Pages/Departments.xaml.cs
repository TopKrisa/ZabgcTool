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
    /// Логика взаимодействия для Departments.xaml
    /// </summary>
    public partial class Departments : Page
    {
        private readonly APIKeys.Core.APIRequest<Model.Data.Departments> API = new APIKeys.Core.APIRequest<Model.Data.Departments>(APIKeys.Core.DataTableNames.Tables.Department, APIKeys.Keys.Api.Admin);
        public Departments()
        {
            InitializeComponent();
            IT.Click += async (s, e) =>
            {
                Model.Data.Departments department = await API.GetDataById(1);
               new HtmlRedactor.Redactor(department.Text).Show();
            };
            Mine.Click += async (s, e) =>
            {
                Model.Data.Departments department = await API.GetDataById(2);
                new HtmlRedactor.Redactor(department.Text).Show();

            };
            Geologist.Click += async (s, e) =>
            {
                Model.Data.Departments department = await API.GetDataById(3);
               new HtmlRedactor.Redactor(department.Text).Show();
            };
        }
    }
}
