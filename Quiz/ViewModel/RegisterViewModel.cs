using Quiz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập password")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string password { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn giới tính")]
        public Gender gender { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập username")]
        [MaxLength(20)]
        public string username { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên đầy đủ")]
        [MaxLength(50)]
        public string fullname { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [MaxLength(50)]
        public string email { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn loại người dùng")]
        public RegisterType type { get; set; }
        public string role { get; set; }

        public int CountQuestion = 0;
        public int CountTest = 0;
        public int CountDoneTest = 0;

        public DateTime? register_date { get; set; }

        [StringLength(200)]
        public string AvatarImage { get; set; }
    }
    public enum RegisterType
    {
        [Display(Name = "Giáo viên")]
        Teacher = 1,
        [Display(Name = "Học sinh / Sinh viên")]
        Student = 2,
    }
}