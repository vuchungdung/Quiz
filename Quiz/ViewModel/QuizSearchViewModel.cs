using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class QuizSearchViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public HardType HardType { get; set; }
    }
}