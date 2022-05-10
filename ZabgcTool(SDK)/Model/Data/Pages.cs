using Newtonsoft.Json;
using System.Net;

namespace ZabgcTool_SDK_.Model.Data
{
    public class Pages
    {
        int id;
        string name;
        string text;
        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return name.Length >= 68 ? WebUtility.HtmlDecode(name.Substring(0, 68).Insert(68, "...")) : WebUtility.HtmlDecode(name);
            }
            set { name = value; }
        }
        [JsonProperty("text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
