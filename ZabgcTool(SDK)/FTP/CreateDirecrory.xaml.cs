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
using System.Windows.Shapes;

namespace ZabgcTool_SDK_.FTP
{
    /// <summary>
    /// Логика взаимодействия для CreateDirecrory.xaml
    /// </summary>
    public partial class CreateDirecrory : Window
    {
        public string NewName;
        public bool IsCreated = false;
        public CreateDirecrory()
        {
            InitializeComponent();
            NameFile.Focus();
            NameFile.SelectAll();
            RenameButton.Click += (s, e) =>
            {
                NewName = NameFile.Text;
                IsCreated = true;
                Close();

            };
            NameFile.KeyDown += (s, e) =>
            {
                if(e.Key == Key.Enter)
                {
                    NewName = NameFile.Text;
                    IsCreated = true;
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
