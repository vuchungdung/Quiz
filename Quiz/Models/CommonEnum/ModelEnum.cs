using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Quiz.Models
{
    public class LimitCountAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public LimitCountAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list == null)
                return false;

            if (list.Count < _min || list.Count > _max)
                return false;

            return true;
        }
    }
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
    public enum CommonStatus
    {
        active = 0,
        notactive = 1,
    }
    public enum DoQuizStatus
    {
        [Display(Name = "Đang làm dở")]
        Doing = 0,
        [Display(Name = "Đã hoàn thành")]
        Finished = 1,
    }
    public enum Gender
    {
        [Display(Name = "Nam")]
        Male = 0,
        [Display(Name = "Nữ")]
        Female = 1,
    }
    public enum UserStatus
    {
        NotActivated = 0,
        Activated = 1
    }
    public enum UserType
    {
        Admin = 0,
        Teacher = 1,
        Student = 2,

    }
    public enum Answer
    {
        [Display(Name = "Đáp án A")]
        A = 0,
        [Display(Name = "Đáp án B")]
        B = 1,
        [Display(Name = "Đáp án C")]
        C = 2,
        [Display(Name = "Đáp án D")]
        D = 3
    }
    public enum TestStatus
    {
        [Display(Name = "Nháp")]
        Draft = 0,
        [Display(Name = "Hiển thị")]
        Active = 1,
    }
    public enum HardType
    {
        [Display(Name = "Dễ")]
        Easy = 0,
        [Display(Name = "Trung bình")]
        Normal = 1,
        [Display(Name = "Khá")]
        QuiteHard = 2,
        [Display(Name = "Khó")]
        Hard = 3,
    }
    public enum QuizStatus
    {
        [Display(Name ="Nháp")]
        NotActive = 0,
        [Display(Name = "Hiển thị")]
        Active = 1,
    }

    public enum TimeQuiz
    {
        [Display(Name = "10 Phút")]
        TenMin = 10,
        [Display(Name = "15 Phút")]
        FifteenMin = 15,
        [Display(Name = "30 Phút")]
        ThirdtyMin = 30,
        [Display(Name = "45 Phút")]
        FortyFiveMin = 45,
        [Display(Name = "60 Phút")]
        SixtyMin = 60,
        [Display(Name = "90 Phút")]
        NineTy = 90,

    }
    

}