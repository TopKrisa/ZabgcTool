using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZabgcTool_SDK_.RemoteDesktop
{
    public partial class RDPCore : Form
    {
        private string _Server;
        private string _UserName;
        private string _Password;
        public RDPCore(string Server, string UserName, string Password, string Pc, string Dns)
        {
            _Server = $"{Pc}.{Dns}";
            _UserName = UserName;
            _Password = Password;
            InitializeComponent();
            RDPClient.OnDisconnected += (s, e) => Close();
            
            NamePc.Text = $" {Pc}.{Dns} ({Server})";
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
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            
            RDPClient.ColorDepth = 32;
            RDPClient.DesktopWidth = resolution.Width-10;
            RDPClient.DesktopHeight = resolution.Height-45;
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
