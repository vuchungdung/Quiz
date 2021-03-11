using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class DoTestViewModel
    {
        public int RoomID { get; set; }
        public string TestName { get; set; }
        public string RoomCode { get; set; }
        public int TotalTime { get; set; }
        public int TotalMark { get; set; }
        public string SubjectName { get; set; }
        public IEnumerable<ShowQuiz> QuizList { get; set; }
    }
    public class ShowQuiz
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string answerA { get; set; }
        public string answerB { get; set; }
        public string answerC { get; set; }
        public string answerD { get; set; }
        ///public Answer trueAnswer { get; set; }
    }
}