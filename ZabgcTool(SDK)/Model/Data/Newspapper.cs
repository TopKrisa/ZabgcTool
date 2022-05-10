using Newtonsoft.Json;
using System;
using System.Net;

namespace ZabgcTool_SDK_.Model.Data
{
    public class Newspapper
    {
        int id;
        string name;
        string size;
        string format;
        string url;
        string date;
        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("name")]
        public string Name
        {
            get { return WebUtility.HtmlDecode(name); }
            set { name = value; }
        }
        [JsonProperty("size")]
        public string Size
        {
            get { return size; }
            set { size = value + " mb"; }
        }
        [JsonProperty("format")]
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        [JsonProperty("url")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        [JsonProperty("datet")]
        public string Date
        {
            get { return date; }
            private set { date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}"; }
        }
    }
}
