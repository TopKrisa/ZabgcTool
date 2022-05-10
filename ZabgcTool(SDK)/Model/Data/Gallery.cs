using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace ZabgcTool_SDK_.Model.Data
{
    public sealed class Gallery
    {
        int id;
        string name;
        List<Photo> photos;
        string category;
        string count;

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
        [JsonProperty("photos")]
        public List<Photo> Photos
        {
            get { return photos; }
            set { photos = value; }
        }
        [JsonProperty("cat")]
        public string Category
        {
            get { return $"Категория: {category}"; }
            set { category = value; }
        }
        public string Count
        {
            get { return $"Количество: {Photos.Count}"; }
            set { count = value; }
        }
    }
}
