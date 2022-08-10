using Newtonsoft.Json;
using System.Net;

namespace ZabgcTool_SDK_.Model
{
    public class Qusestions
    {
        int id;
        string question;
        string user;
        int act;
        string answer;
        string date;
        string answerdate;
        string NameAdm = new ZabgcTool_SDK_.Helper.Settings().ReadSettings().AdminName;

        public Qusestions() { }


        [JsonProperty("Question")]
        public string Question
        {
            get
            {
                return question.Length >= 100 ? WebUtility.HtmlDecode(question.Substring(0, 100).Insert(100, "...")):WebUtility.HtmlDecode(question);
            }
            set { question = value; }
        }
        public string QuestionPreview
        {
            get
            {
                return question.Length >= 100 ? WebUtility.HtmlDecode(question.Substring(0, 100).Insert(100, "...")) : WebUtility.HtmlDecode(question);
            }
            
        }
        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty("User")]
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        [JsonProperty("Activity")]
        public int Activity
        {
            get { return act; }
            set { act = value; }
        }
        [JsonProperty("Answer")]
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        [JsonProperty("Date")]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        [JsonProperty("AnswerDate")]
        public string AnswerDate
        {

            get
            {
                return !string.IsNullOrEmpty(answerdate) ? answerdate : "Требует редакции";
            }
            set { answerdate = value; }
        }
        [JsonProperty("Name")]
        public string adminname
        {
            get { return answerdate; }
            set { answerdate = value; }
        }



        public Qusestions(string question, string user, int activity, string answer, string date, string answerDate, int _Id)
        {
            Id = _Id;
            Question = question;
            User = user;
            Activity = activity;
            Answer = answer;
            Date = date;
            AnswerDate = answerDate;
            adminname = NameAdm;
        }


    }
}
