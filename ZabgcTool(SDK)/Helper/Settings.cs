using Newtonsoft.Json;
using System.IO;

namespace ZabgcTool_SDK_.Helper
{
    public class Settings
    {
        [JsonProperty("Addres")]
        public string Addres;
        [JsonProperty("Login")]
        public string Login;
        [JsonProperty("Password")]
        public string Password;
        [JsonProperty("StayOnline")]
        public bool StayOnline;
        [JsonProperty("AdminName")]
        public string AdminName;
        [JsonProperty("ShedulePath")]
        public string ShedulePath;
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Autorun")]
        public bool Autorun;
        [JsonProperty("APIPath")]
        public string APIPath;
        [JsonProperty("ControlData")]
        public string ControlData;
        [JsonProperty("Type")]
        public int UserType;
        public Settings()
        {
        }

        public Settings(string addres, string login, string password, bool stayOnline, string adminName, string shedulePath, string name, bool autorun, string api, string controlData, int Type)
        {
            Addres = addres;
            Login = login;
            Password = password;
            StayOnline = stayOnline;
            AdminName = adminName;
            ShedulePath = shedulePath;
            Name = name;
            Autorun = autorun;
            APIPath = api;
            ControlData = controlData;
            UserType = Type; 
        }
        public void SetType(int Type)
        {
            var settings = new Settings();
            string Json = null;
            using (var reader = new StreamReader($@"{Directory.GetCurrentDirectory()}\Settings.json"))
            {
                Json = reader.ReadToEnd();
            }
            settings = JsonConvert.DeserializeObject<Settings>(Json);
            new Settings(settings.Addres, settings.Login, settings.Password, settings.StayOnline, AdminName, ShedulePath, Name, settings.Autorun, settings.APIPath, settings.ControlData,Type).SaveSettings();

        }
        public void SetCheckStayOnline(bool check, string ControlData)
        {
            var settings = new Settings();
            string Json = null;
            using (var reader = new StreamReader($@"{Directory.GetCurrentDirectory()}\Settings.json"))
            {
                Json = reader.ReadToEnd();
            }
            settings = JsonConvert.DeserializeObject<Settings>(Json);
            new Settings(settings.Addres, settings.Login, settings.Password, check,AdminName, ShedulePath,settings.Name, settings.Autorun, settings.APIPath, ControlData, settings.UserType).SaveSettings();
        }
        public void SetName(string Name)
        {
            var settings = new Settings();
            string Json = null;
            using (var reader = new StreamReader($@"{Directory.GetCurrentDirectory()}\Settings.json"))
            {
                Json = reader.ReadToEnd();
            }
            settings = JsonConvert.DeserializeObject<Settings>(Json);
            new Settings(settings.Addres, settings.Login, settings.Password, settings.StayOnline, AdminName, ShedulePath, Name, settings.Autorun, settings.APIPath, settings.ControlData, settings.UserType).SaveSettings();
        }
        public async void SaveSettings()
        {
            var settings = new Settings(Addres, Login, Password, StayOnline, AdminName, ShedulePath,Name, Autorun, APIPath, ControlData,UserType);
            var Json = JsonConvert.SerializeObject(settings);
            using(var writer = new StreamWriter($@"{Directory.GetCurrentDirectory()}\Settings.json"))
            {
               await writer.WriteAsync(Json);
            }
            Helper.SetAutorunValue(Autorun);
        }
        public Settings ReadSettings()
        {
            var settings = new Settings();
            string Json=null;
            using (var reader = new StreamReader($@"{Directory.GetCurrentDirectory()}\Settings.json"))
            {
                Json = reader.ReadToEnd();
            }
            return settings = JsonConvert.DeserializeObject<Settings>(Json);
        }
    }

}
