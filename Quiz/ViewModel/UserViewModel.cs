using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public UserType UserType { get; set; }
        public UserStatus Status { get; set; }
    }
}