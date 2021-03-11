using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public partial class QuizContext : DbContext
    {
        public QuizContext()
            : base("QuizContext")
        {
        }
        public virtual DbSet<Quizs> Quizzes { get; set; }
        public virtual DbSet<QuizTest> QuizTests { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<ActiveTest> ActiveTests { get; set; }
        public virtual DbSet<QuizResult> QuizResults { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ////base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<QuizTest>()
                .HasMany<Quizs>(e => e.Quiz)
                .WithMany(e => e.QuizTest)
                .Map(m => m.ToTable("Quiz_Of_Test")
                .MapLeftKey("TestID")
                .MapRightKey("QuizID"));

            modelBuilder.Entity<User>()
                .HasMany(i => i.QuizTests)
                .WithOptional()
                .WillCascadeOnDelete(false);
                
            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Entity<ActiveTest>()
                .HasMany(c => c.QuizResults)
                .WithRequired(c => c.ActiveTest)
                .WillCascadeOnDelete(false);


        }
    }
}