using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Quizs
    {
        public Quizs()
        {
            QuizTest = new HashSet<QuizTest>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizID { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [ForeignKey("Creator")]
        public int CreatorID { get; set; }
        public virtual User Creator { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [StringLength(500)]
        [Required]
        public string content { get; set; }

        [Required]
        public HardType HardType { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }

        public virtual Subject Subject { get; set; }

        [StringLength(200)]
        [Required]
        public string answerA { get; set; }
        [StringLength(200)]
        [Required]
        public string answerB { get; set; }
        [StringLength(200)]
        [Required]
        public string answerC { get; set; }
        [StringLength(200)]
        [Required]
        public string answerD { get; set; }
        [Required]
        public Answer trueAnswer { get; set; }

        public virtual ICollection<QuizTest> QuizTest { get; set; }

    }
    
}