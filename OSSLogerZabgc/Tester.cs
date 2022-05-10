using SpeedTest;
using SpeedTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace OSSLogerZabgc
{
    public static class Tester
    {
       
        public static  List<string>  GetDiskInfo()
        {
            List<string> Data = new List<string>();
            WqlObjectQuery query = new WqlObjectQuery(
                   "Select * from Win32_DiskPartition");
            ManagementObjectSearcher find =
                new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in find.Get())
            {

                Data.Add("{\nBlock size." + mo["BlockSize"] + " Bytes \n" +
                   "Partition is labeled as bootable. " + mo["Bootable"] + "\n" +
                  "Boot partition active. " + mo["BootPartition"] + "\n" +
                  "Caption.." + mo["Caption"] + "\n" +
                  "Description." + mo["Description"] + "\n" +
                  "Unique identification of partition.." + mo["DeviceID"] + "\n" +
                  "Index number of the disk with that partition." + mo["DiskIndex"] + "\n" +
                  "Detailed description of error in LastErrorCode." + mo["ErrorDescription"] + "\n" +
                  "Type of error detection and correction." + mo["ErrorMethodology"] + "\n" +
                  "Hidden sectors in partition." + mo["HiddenSectors"] + "\n" +
                  "Index number of the partition." + mo["Index"] + "\n" +
                  "Last error by device." + mo["LastErrorCode"] + "\n" +
                  "Total number of consecutive blocks." + mo["NumberOfBlocks"] + "\n" +
                  "Partition labeled as primary." + mo["PrimaryPartition"] + "\n" +
                  "Free description of media purpose. " + mo["Purpose"] + "\n" +
                  "Total size of partition." + mo["Size"] + " bytes" + "\n" +
                  "Starting offset of the partition " + mo["StartingOffset"] + "\n" +
                  "Status." + mo["Status"] + "\n" +
                  "Type of the partition." + mo["Type"] + "\n}" );
            }
            return Data;
        }
        public static bool GetInternetInfo(string HostName)
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
        public static string GetStartedServices()
        {
          Process process =  Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                
                Arguments = "/c chcp 65001 & net stats workstation",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            return  process.StandardOutput.ReadToEnd();
        }    
    }
}
