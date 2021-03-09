namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_101 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Quizs", "image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quizs", "image", c => c.String(maxLength: 100));
        }
    }
}
