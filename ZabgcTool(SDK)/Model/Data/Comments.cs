using Newtonsoft.Json;
using System.Net;

namespace ZabgcTool_SDK_.Model
{
    public class Comments
    {
        int _id;
        string _message;
        string _date;
        int act;
        string ip;
        string answer;
        string name;
        string otvet;
        public Comments() { }

        public Comments(int id, string message, string date, int act, string ip, string answ, string _Name, string _Otvet)
        {

            Id = id;
            Message = message;
            Date = date;
            Activate = act;
            IP = ip;
            Answer = answ;
            Name = _Name;
            Otvet = _Otvet;
        }

        public Comments(int id, string message, string answer, string name)
        {
            Id = id;
            Message = message;
            Answer = answer;
            Name = name;
        }

        [JsonProperty("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        [JsonProperty("message")]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        [JsonProperty("date")]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }
        [JsonProperty("act")]
        public int Activate
        {
            get { return act; }
            set { act = value; }
        }
        [JsonProperty("ip")]
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        [JsonProperty("Answer")]
        public string Answer
        {
            get { return WebUtility.HtmlDecode(answer); }
            set { answer = value; }
        }
        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty("otvet")]
        public string Otvet
        {
            get { return otvet; }
            set { otvet = value; }
        }

    }
}
