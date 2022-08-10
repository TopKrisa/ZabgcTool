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
        [JsonProperty("Type")]
        public int Type;
        public Authorization(string login, string password)
        {
            LoginName = login;
            Password = password;
        }
        public Authorization()
        {

        }
        public (bool,int) Login()
        {
            var users = GetUsers();
            bool IsLogin = false;
            int type=99999;
            foreach(var user in users)
            {

                if(user.LoginName == LoginName && user.Password == Password)
                {
                    IsLogin = true;
                    type = user.Type;
                    break;
                }
                else
                {
                    IsLogin = false;
                    type = user.Type;
                }
            }
            return (IsLogin,type);
        }
        public bool CheckUserUserWithThisName(string Name)
        {
            return GetUsers().FirstOrDefault(x => x.LoginName == Name)!= null ? true : false;
        }
        private List<Authorization> GetUsers()
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(
             $"https://zabgc.zabedu.ru/api/users/={new APIKeys.Keys(APIKeys.Keys.Api.Admin).Key}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<Authorization>>(json);
        }
        public enum UserTypes
        {
            Admin =0,
            ScheduleAdmin = 1,

        }
    }
}
