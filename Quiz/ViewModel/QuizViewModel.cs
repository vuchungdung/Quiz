using Quiz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.ViewModel
{
    public class QuizViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đầy đủ tiêu đề!")]
        public string Name { get; set; }

        public int CreatorID { get; set; }

        public virtual User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đầy đủ câu hỏi!")]
        public string content { get; set; }

        public HardType HardType { get; set; }

        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đáp án A!")]
        public string answerA { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đáp án B!")]
        public string answerB { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đáp án C!")]
        public string answerC { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đáp án D!")]
        public string answerD { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn câu trả lời đúng!")]
        public Answer trueAnswer { get; set; }
    }
}