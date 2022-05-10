using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace OSSLogerZabgc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random r = new Random();
        public MainWindow()
        {
            InitializeComponent();
            IP.Content = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            Pc_Name.Content = Dns.GetHostName();
            DNS.Content = string.Empty;
            foreach (var dns in GetDnsAdresses(true, false))
            {
                DNS.Content += dns.ToString() + "\n";
            }
        }

        private async void butn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void CheckPC_Click(object sender, RoutedEventArgs e)
        {
           
            var PCList = await new API.Core.APIRequest<PCModel>(API.Core.DataTableNames.Tables.OSSLogger, API.Core.Keys.Api.Admin).GetData();
            var pc = PCList.FirstOrDefault(x => x.IP == Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString());
            if (pc != null)
            {
                await new API.Core.APIRequest<PCModel>(API.Core.DataTableNames.Tables.OSSLogger, API.Core.Keys.Api.Admin).EditData(
                    new PCModel()
                    {
                        IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(),
                        Name = Dns.GetHostName(),
                        DNS = GetDnsAdresses(true, false)[0].ToString(),
                        Condition = $"SystemData : {HealthCheck()[2]}\nInternetData : {Tester.GetInternetInfo("zabedu.ru")}\n ServiceData = {Tester.GetStartedServices()}64 bit ?: {Environment.Is64BitOperatingSystem} \n OS Version:{Environment.OSVersion}"

                    }, pc.Id);
                Panel.Visibility = Visibility.Visible;
            }
            else
            {
                await new API.Core.APIRequest<PCModel>(API.Core.DataTableNames.Tables.OSSLogger, API.Core.Keys.Api.Admin).AddData(new PCModel()
                {
                    IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(),
                    Name = Dns.GetHostName(),
                    DNS = GetDnsAdresses(true, false)[0].ToString(),
                    Condition = $"SystemData : { HealthCheck()[2]}\nInternetData: { Tester.GetInternetInfo("zabedu.ru")}\n ServiceData = { Tester.GetStartedServices() }64 bit ?: { Environment.Is64BitOperatingSystem} \n OS Version: { Environment.OSVersion}"
                });
                Panel.Visibility = Visibility.Visible;
            }
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",

                Arguments = "/c chcp 65001 & reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\"" +
                " /v fDenyTSConnections /t REG_DWORD /d 0 /f & netsh advfirewall firewall add rule name=\"allow RDP\"" +
                " dir=in protocol=TCP localport=3389 action=allow",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });


        }
        private List<string> HealthCheck()
        {
            EventLog myLog; //Можно читать и записывать элементы журнала событий
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\ServiceLog.txt"; //Хранится путь к файлу
            List<string> list = new List<string>(); //Список всех найденных ошибок
            myLog = new EventLog();
            myLog.Log = "System"; //По умолчанию выставлен тип ошибок - приложение(Application), а нам нужны системные(System).
            myLog.Source = "System Error";

            for (int index = myLog.Entries.Count - 1; index > 0; index--) //Перебираем все системные ошибки от новых к более старым
            {
                var errEntry = myLog.Entries[index]; //Чтение записи из журнала событий
                if (errEntry.EntryType == EventLogEntryType.Error) //Проверка ошибка ли это
                {
                    //Получаем нужные данные из записи
                    var appName = errEntry.Source;
                    list = new List<string>();
                    list.Add("Entry Type: " + Convert.ToString(errEntry.EntryType));
                    list.Add("Event log: " + (string)myLog.Log);
                    list.Add("Machine Name: " + (string)errEntry.MachineName);
                    list.Add("App Name: " + (string)errEntry.Source);
                    list.Add("Message: " + (string)errEntry.Message);
                    list.Add("Time Written: " + errEntry.TimeWritten.ToString());
                    list.Add("-*-");
                }
            }
            return list;
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
        public static IPAddress[] GetDnsAdresses(bool ip4Wanted, bool ip6Wanted)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            HashSet<IPAddress> dnsAddresses = new HashSet<IPAddress>();

            foreach (NetworkInterface networkInterface in interfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                    foreach (IPAddress forAddress in ipProperties.DnsAddresses)
                    {
                        if ((ip4Wanted && forAddress.AddressFamily == AddressFamily.InterNetwork) || (ip6Wanted && forAddress.AddressFamily == AddressFamily.InterNetworkV6))
                        {
                            dnsAddresses.Add(forAddress);
                        }
                    }
                }
            }

            return dnsAddresses.ToArray();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            System.Diagnostics.Process.Start($"http://zabgc.zabedu.ru");
        }
    }
}
