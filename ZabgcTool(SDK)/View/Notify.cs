using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZabgcTool_SDK_.View
{
    public class Notify
    {
        string notifyTEXT;
        string notifyDATE;
        string type;
        int isseen;
        [JsonProperty("NotifyMessage")]
        public string NotifyText
        {
            get { return notifyTEXT; }
            set { notifyTEXT = value; }
        }
        [JsonProperty("NotifyDate")]
        public string NotifyDate
        {
            get { return notifyDATE; }
            set { notifyDATE = value; }
        }
        [JsonProperty("IsSeen")]
        public int IsSeen
        {
            get { return isseen; }
            set { isseen = value; }
        }
        [JsonProperty("Type")]
        public string Type
            {
            get { return type; }
            set { type = value;}
            }
        public Notify(string notifytext)
        {
            NotifyText = notifytext;
            NotifyDate = DateTime.Now.ToShortDateString()+" " + DateTime.Now.ToShortTimeString();
        }
        public Notify()
        {

        }

        public void AddNotify(string message)
        {
            var notify = new  Notify(message);
          
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://zabgc.ru/api/notify/={new APIKeys.Keys(APIKeys.Keys.Api.Admin).Key}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = JsonConvert.SerializeObject(notify);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            
        }
    }
}
