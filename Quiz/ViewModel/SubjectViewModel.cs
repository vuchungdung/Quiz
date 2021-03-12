using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập đầy đủ tên môn học!")]
        public string Name { get; set; }

        public int Count { get; set; }
    }
}