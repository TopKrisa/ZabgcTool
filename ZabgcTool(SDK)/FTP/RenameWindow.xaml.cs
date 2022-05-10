using System.IO;
using System.Net;
using System.Windows;

namespace ZabgcTool_SDK_.FTP
{
    /// <summary>
    /// Логика взаимодействия для RenameWindow.xaml
    /// </summary>
   
    public partial class RenameWindow : Window
    {
        public string NewName;
        public bool IsRenamed = false;
        public RenameWindow(string CurrentName)
        {
            InitializeComponent();
            NameFile.Text = CurrentName;
            NameFile.Focus();
            NameFile.SelectAll();
            RenameButton.Click += (s, e) => 
            {
                NewName = Path.GetFileNameWithoutExtension(NameFile.Text) + Path.GetExtension(CurrentName);
                IsRenamed = true;
                Close();

            };
            NameFile.KeyDown += (s, e) =>
            {
                if(e.Key == System.Windows.Input.Key.Enter)
                {
                    NewName = Path.GetFileNameWithoutExtension(NameFile.Text) + Path.GetExtension(CurrentName);
                    IsRenamed = true;
                    Close();
                }
            };
            BackBTN.Click += (s, e) =>
             {
                 Close();
             };
        }
       
    }
}
