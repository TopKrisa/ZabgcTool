using Newtonsoft.Json;
using System;
using System.Windows.Media.Imaging;

namespace ZabgcTool_SDK_.Model.Data
{
    public class News
    {
        int id;
        string name;
        string date;
        string poster;
        BitmapImage image;
        string description;
        string message;
        string views;
        string comments;

        public News()
        {
        }

        public News(int id, string name, string date, string poster, string description, string message, string views, string comments)
        {
            Id = id;
            Name = name;
            Date = date;
            Poster = poster;
            Description = description;
            Message = message;
            Views = views;
            Comments = comments;
        }

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
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        [JsonProperty("poster")]
        public string Poster
        {
            get { return poster; }
            set { poster = value; }
        }
        [JsonProperty("description")]
        public string Description
        {
            get { return description.Length >= 138 ? description.Substring(0, 138).Insert(138, "...") : description; }
            set { description = value; }
        }
        [JsonProperty("message")]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [JsonProperty("view")]
        public string Views
        {
            get { return $"Просмотры: {views}"; }
            set { views = value; }
        }
        [JsonProperty("comment")]
        public string Comments
        {
            get { return $"Комментарии: {comments}"; }
            set { comments = value; }
        }
        public BitmapImage Preview
        {
            get
            {
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("http://zabgc.ru/" + Poster);
                bi.EndInit();
                return bi;
            }

        }
    }
}
