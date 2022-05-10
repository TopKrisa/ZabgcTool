using System;
using System.Windows.Forms;

namespace ZabgcTool_SDK_.RemoteDesktop
{
    public partial class RDPCore : Form
    {
        private string _Server;
        private string _UserName;
        private string _Password;
        public RDPCore(string Server, string UserName, string Password, string Pc)
        {
            _Server = Server;
            _UserName = UserName;
            _Password = Password;
            InitializeComponent();
            NamePc.Text = $"{Pc}({Server})";
        }
        public RDPCore()
        {
            InitializeComponent();
           
        }

        private void RDPCore_SizeChanged(object sender, EventArgs e)
        {
            // ClientRPD.DesktopWidth = Width - 30;
            // ClientRPD.DesktopHeight = this.Height - 40;
        }

        private void RDPCore_Load(object sender, EventArgs e)
        {
            RDPClient.Server = _Server;
            RDPClient.UserName = _UserName;
            RDPClient.AdvancedSettings2.ClearTextPassword = _Password;
            RDPClient.AdvancedSettings7.EnableCredSspSupport = true;

            RDPClient.ColorDepth = 32;
            RDPClient.DesktopWidth = 1900;
            RDPClient.DesktopHeight = 1040;
            RDPClient.Connect();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (RDPClient.Connected.ToString() == "1")
                RDPClient.Disconnect();
            base.OnClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            RDPClient.Server = _Server;
            RDPClient.UserName = _UserName;
            RDPClient.AdvancedSettings2.ClearTextPassword = _Password;
            RDPClient.AdvancedSettings7.EnableCredSspSupport = true;
            RDPClient.ColorDepth = 32;
            RDPClient.DesktopWidth = 1900;
            RDPClient.DesktopHeight = 1040;
            RDPClient.Connect();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
