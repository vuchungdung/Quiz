using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Subject
    {
        public Subject()
        {
            QuizTests = new HashSet<QuizTest>();
            Quizzes = new HashSet<Quizs>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        public string name { get; set; }

        public ICollection<Quizs> Quizzes { get; set; }

        public ICollection<QuizTest> QuizTests { get; set; }
    }
}