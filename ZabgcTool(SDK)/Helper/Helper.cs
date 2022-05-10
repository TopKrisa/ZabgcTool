using Microsoft.Win32;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows;
namespace ZabgcTool_SDK_.Helper
{
    public static class Helper
    {
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? System.Windows.Application.Current.Windows.OfType<T>().Any()
               : System.Windows.Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        public static bool SetAutorunValue(bool autorun)
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            string ExePath = Assembly.GetExecutingAssembly().Location;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue(name, ExePath);
                else
                    reg.DeleteValue(name);

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool CheckInternet(string HostName)
        {
            try
            {
                Ping ping = new Ping();
                PingReply Status = ping.Send(HostName, 10000);
                return Status.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
