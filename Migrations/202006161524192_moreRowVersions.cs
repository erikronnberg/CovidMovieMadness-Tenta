namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreRowVersions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Post", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "RowVersion");
            DropColumn("dbo.Comment", "RowVersion");
        }
    }
}
