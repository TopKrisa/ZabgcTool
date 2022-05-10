using Newtonsoft.Json;

namespace ZabgcTool_SDK_.Model.Data
{
    public class Main
    {

        [JsonProperty("totalMaterital")]
        public string Materials { get; set; }
        [JsonProperty("totalPages")]
        public string Pages { get; set; }
        [JsonProperty("totalUsers")]
        public string Users { get; set; }
        [JsonProperty("totalPhotoAbums")]
        public string PhotoAlbums { get; set; }

        public Main() { }


    }
}
