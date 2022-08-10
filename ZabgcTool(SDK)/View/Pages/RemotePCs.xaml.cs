using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для RemotePCs.xaml
    /// </summary>
    public partial class RemotePCs : Page
    {
        public RemotePCs(TextBlock text)
        {
            InitializeComponent();
            text.Text = "OSS/RDP";
            Loaded += async (s, e) =>
            {
                List<Model.Data.RemotePC> PC = await new APIKeys.Core.APIRequest<Model.Data.RemotePC>(APIKeys.Core.DataTableNames.Tables.OSSLogger, APIKeys.Keys.Api.Admin).GetData();
                PC = PC.Where(x => x.IP != Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()).ToList();
                if(PC.Count == 0)
                {
                    NoPCData.Visibility = Visibility.Visible;
                
                }
                else
                {
                    Computers.DataContext = PC;
                }
                
            };
        }
        
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.RemotePC remotePC = (Model.Data.RemotePC)(sender as Button).DataContext;
            if (!System.Windows.Forms.Application.OpenForms.OfType<RemoteDesktop.RDPCore>().Any())
            {
                new RemoteDesktop.RDPCore(remotePC.IP, "", "",remotePC.Name, remotePC.DNS).Show();
            }
        }

        private void ViewInfo_Click(object sender, RoutedEventArgs e)
        {
            Model.Data.RemotePC remotePC = (Model.Data.RemotePC)(sender as Button).DataContext;
            if (!Helper.Helper.IsWindowOpen<PCInformation>())
            {
                new PCInformation(remotePC.Condition, remotePC.IP+" - "+ remotePC.Name).Show();
            }
        }
    }
}
