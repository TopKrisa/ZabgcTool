using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSLogerZabgc
{
    public class PCModel
    {
     [JsonProperty("Id")]
      public  int Id;
        [JsonProperty("IP")]
        public  string IP;
        [JsonProperty("DNS")]
        public  string DNS;
        [JsonProperty("Name")]
        public  string Name;
        [JsonProperty("Condition")]
        public  string Condition;

    }
}
