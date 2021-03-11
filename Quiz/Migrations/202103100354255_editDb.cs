namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDb : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "username" });
            DropIndex("dbo.Users", new[] { "email" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "email", unique: true);
            CreateIndex("dbo.Users", "username", unique: true);
        }
    }
}
