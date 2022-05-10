using Newtonsoft.Json;

namespace ZabgcTool_SDK_.Model.Data
{
    public class Departments
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
            get { return name; }
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
