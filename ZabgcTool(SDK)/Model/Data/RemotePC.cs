using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabgcTool_SDK_.Model.Data
{
    public class RemotePC
    {
        int id;
        string Ip;
        string Dns;
        string name;
        string condition;
        [JsonProperty("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("IP")]
        public string IP
        {
            get { return Ip; }
            set { Ip = value; }
        }
        [JsonProperty("DNS")]
        public string DNS
        {
            get { return Dns; }
            set { Dns = value; }
        }
        [JsonProperty("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty("Condition")]
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }
    }
}
