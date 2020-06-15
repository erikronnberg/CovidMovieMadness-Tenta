namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "PostDate");
        }
    }
}
