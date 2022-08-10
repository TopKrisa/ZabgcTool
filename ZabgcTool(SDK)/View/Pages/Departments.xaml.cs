using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZabgcTool_SDK_.HtmlRedactor;

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
                SaveChangesAsync(department);
            };
            Mine.Click += async (s, e) =>
            {
                Model.Data.Departments department = await API.GetDataById(2);

                 SaveChangesAsync(department);
            };
            Geologist.Click += async (s, e) =>
            {
                Model.Data.Departments department = await API.GetDataById(3);
                 SaveChangesAsync(department);
            };

        }
        private  void SaveChangesAsync(Model.Data.Departments department)
        {
            var Editor = new HtmlRedactor.EditRedactor(department.Text,async ()=> {

               
               await API.EditData(new Model.Data.Departments() { Id = department.Id, Text = Saver.Text, Name = department.Name }, department.Id);
            });
            Editor.Show();
            //    Editor.Closing += async (se, ee) =>
            //    {
            //        var _editedText = await Editor.SaveAsync();
            //      _editedText = WebUtility.HtmlDecode(_editedText.Replace("\r\n", string.Empty));
            //        var _notEditedText = WebUtility.HtmlDecode(department.Text.Replace("\r\n", string.Empty));

            //        _notEditedText = _notEditedText.Replace("\n", string.Empty);

            //        if (_notEditedText != _editedText)
            //        {

            //            MessageBox.Show(_editedText);

            //        }
            //        else
            //        {
            //            MessageBox.Show("test");
            //        }
            ////           
            //    };
        }
    }
}
