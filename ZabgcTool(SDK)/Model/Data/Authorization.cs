using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace ZabgcTool_SDK_.Model
{
    public class Authorization
    {
        [JsonProperty("Login")]
        public string LoginName;
        [JsonProperty("Password")]
        public string Password;

        public Authorization(string login, string password)
        {
            LoginName = login;
            Password = password;
        }
        public Authorization()
        {

        }
        public bool Login()
        {
            var users = GetUsers();
            bool IsLogin = false;
            foreach(var user in users)
            {
                if(user.LoginName == LoginName && user.Password == Password)
                {
                    IsLogin = true;
                }
                else
                {
                    IsLogin = false;
                }
            }
            return IsLogin;
        }
        public bool CheckUserUserWithThisName(string Name)
        {
            return GetUsers().FirstOrDefault(x => x.LoginName == Name)!= null ? true : false;
        }
        private List<Authorization> GetUsers()
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(
             $"http://zabgc.ru/api/users/={new APIKeys.Keys(APIKeys.Keys.Api.Admin).Key}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<Authorization>>(json);
        }
    }
}
