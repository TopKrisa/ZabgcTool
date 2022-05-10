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
    /// Логика взаимодействия для DragAlert.xaml
    /// </summary>
    public partial class DragAlert : Window
    {
        public DragAlert()
        {
            InitializeComponent();
        }

        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("sasa");
        }
    }
}
