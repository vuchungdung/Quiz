namespace Quiz.Migrations
{
    using Quiz.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Quiz.Models.QuizContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Quiz.Models.QuizContext context)
        {
            context.Users.AddOrUpdate(e => e.ID, new User
            {
                ID = 1,
                username = "admin",
                email = "admin@gmail.com",
                fullname = "Admin",
                type = UserType.Admin,
                role = "admin",
                status = UserStatus.Activated,
                register_date = DateTime.Parse("2019-09-09"),
                AvatarImage = "default-avatar.jpg",
                password = "0192023A7BBD73250516F069DF18B500", // = admin123
                gender = Gender.Female,
            });
            context.Users.AddOrUpdate(e => e.ID, new User
            {
                ID = 2,
                username = "teacher1",
                email = "teacherdemo@gmail.com",
                fullname = "Giao Viên 1",
                type = UserType.Teacher,
                role = "teacher",
                status = UserStatus.Activated,
                register_date = DateTime.Parse("2019-09-09"),
                AvatarImage = "default-avatar.jpg",
                password = "0192023A7BBD73250516F069DF18B500", // = admin123
                gender = Gender.Male,
            });
            context.Users.AddOrUpdate(e => e.ID, new User
            {
                ID = 3,
                username = "student1",
                email = "studentdemo@gmail.com",
                fullname = "Sinh Viên 1",
                type = UserType.Student,
                role = "student",
                status = UserStatus.Activated,
                register_date = DateTime.Parse("2019-09-09"),
                AvatarImage = "default-avatar.jpg",
                password = "0192023A7BBD73250516F069DF18B500", // = admin123
                gender = Gender.Male,
            });
        }
    }
}
