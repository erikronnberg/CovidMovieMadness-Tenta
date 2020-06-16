namespace CovidMovieMadness___Tenta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class erroMessages : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Post", "PostTitle", c => c.String(nullable: false, maxLength: 100));
        }

        public override void Down()
        {
            AlterColumn("dbo.Post", "PostTitle", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Comment", "Username", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
