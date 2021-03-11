using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class SubmitExamViewModel
    {
        public int QuizID { get; set; }
        public SubmitAnswer Answer { get; set; }
    }
    public enum SubmitAnswer
    {
        [Display(Name = "Đáp án A")]
        A = 0,
        [Display(Name = "Đáp án B")]
        B = 1,
        [Display(Name = "Đáp án C")]
        C = 2,
        [Display(Name = "Đáp án D")]
        D = 3,
        NotCheck = -1,
    }
}