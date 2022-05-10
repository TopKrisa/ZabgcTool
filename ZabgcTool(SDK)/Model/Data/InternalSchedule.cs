using Newtonsoft.Json;

namespace ZabgcTool_SDK_.Model.Data
{
    public class InternalSchedule
    {
        int id;
        string name;
        string format;
        string url;
        double size;
        string Jsonsize;
        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty("url")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        [JsonProperty("format")]
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        public double FileSize
        {
            set { size = value; }
        }
        [JsonProperty("size")]
        public string JsonSize
        {
            get { return Jsonsize; }
            set { Jsonsize = value; }
        }
    }
}
