namespace CovidMovieMadness___Tenta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class postTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "PostRating", c => c.Int(nullable: false));
            AddColumn("dbo.Post", "PostTitle", c => c.String(nullable: false, maxLength: 1000));
            DropColumn("dbo.Post", "ReviewRating");
        }

        public override void Down()
        {
            AddColumn("dbo.Post", "ReviewRating", c => c.Int(nullable: false));
            DropColumn("dbo.Post", "PostTitle");
            DropColumn("dbo.Post", "PostRating");
        }
    }
}
