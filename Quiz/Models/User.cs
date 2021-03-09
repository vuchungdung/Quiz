using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class User
    {
        public User()
        {
            Quizzes = new HashSet<Quizs>();
            QuizTests = new HashSet<QuizTest>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public UserStatus status { get; set; }

        [Required]
        [StringLength(500)]
        public string password { get; set; }

        public Gender gender { get; set; }

        [Required]
        [StringLength(20)]
        public string username { get; set; }

        [StringLength(100)]
        public string fullname { get; set; }

        [StringLength(100)]
        public string email { get; set; }
        [Required]
        public UserType type { get; set; }

        [Required]
        [StringLength(15)]
        public string role { get; set; }

        public DateTime? register_date { get; set; }

        [StringLength(200)]
        public string AvatarImage { get; set; }

        public ICollection<Quizs> Quizzes { get; set; }
        public ICollection<QuizTest> QuizTests { get; set; }

    }
   
}