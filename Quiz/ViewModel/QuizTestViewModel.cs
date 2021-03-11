using Quiz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.ViewModel
{
    public class QuizTestViewModel
    {
        public int TestID { get; set; }

        [Required]
        public TimeQuiz TotalTime { get; set; }

        [Required]
        [Range(10, 1000)]
        public int TotalMark { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public int CreatorID { get; set; }

        public virtual User Creator { get; set; }

        public string SubjectName { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public TestStatus status { get; set; }

        public IEnumerable<SelectListItem> Subject { get; set; }

        [Required(ErrorMessage = "Chưa chọn môn học")]
        public int SubjectID { get; set; }

        public HardType HardTypeChoose { get; set; }

        [LimitCount(1, 50, ErrorMessage = "Số câu hỏi từ 1 tới 50")]
        public List<int> quizID { get; set; }
    }
}