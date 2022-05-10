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

namespace ZabgcTool_SDK_.View
{
    /// <summary>
    /// Логика взаимодействия для PCInformation.xaml
    /// </summary>
    public partial class PCInformation : Window
    {
        public PCInformation(string Information, string IP)
        {

            InitializeComponent();
            this.Information.Text = Information;
            this.IpPC.Text = IP;
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
