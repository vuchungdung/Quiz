using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class QuizResultViewModel
    {
        public string StudentName { get; set; }
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public DateTime SubmitDate { get; set; }
        public DoQuizStatus Status { get; set; }
        public double Score { get; set; }
        public double MaxScore { get; set; }
        public string Point { get; set; } //Ví dụ: 16/100 điểm
    }
}