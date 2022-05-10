using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZabgcTool_SDK_.APIKeys
{
    public  class APIResponse
    {
         string message;
         bool status;



        [JsonProperty("message")]
        public string Message
        {
            get { return message; }
            set { message = value; }

        }
        [JsonProperty("status")]
        public string Status
        {
            get { return message; }
            set { message = value; }

        }
        public APIResponse ConvertResponseAsync(HttpWebResponse response)
        {
            using (StreamReader Json = new StreamReader(response.GetResponseStream()))
            {
                return JsonConvert.DeserializeObject<APIResponse>(Json.ReadToEnd());
            }
        }
    }
}
