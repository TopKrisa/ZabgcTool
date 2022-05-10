using Newtonsoft.Json;
using System;
using System.Windows.Media.Imaging;

namespace ZabgcTool_SDK_.Model.Data
{
    public class Photo
    {
        int id;
        int id_album;
        string original;
        string date;
        string size;
        string format;
        string helperOriginal = "http://zabgc.ru";
        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("id_albom")]
        public int Id_Album
        {
            get { return id_album; }
            set { id_album = value; }
        }

        [JsonProperty("original")]
        public string Original
        {
            get
            {
                return !original.Contains("http://zabgc.ru") ? $"http://zabgc.ru{original}" : original;
            }
            set { original = value; }
        }
        [JsonProperty("datet")]
        public string Date
        {
            get { return date; }
            set { date = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}"; }
        }
        [JsonProperty("size_photo")]
        public string Size
        {
            get { return size; }
            set { size = value; }
        }
        [JsonProperty("formatt")]
        public string Format
        {
            get { return format; }
            set { format = value.ToLower(); }
        }
        public BitmapImage Preview
        {
            get
            {
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Original);
                bi.EndInit();
                return bi;
            }

        }
    }
}
