namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class innitCommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 1000),
                        CommentContent = c.String(nullable: false, maxLength: 1000),
                        UserRating = c.Int(nullable: false),
                        Post_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Post", t => t.Post_ID)
                .Index(t => t.Post_ID);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Genre = c.String(nullable: false, maxLength: 50),
                        Year = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ReviewRating = c.Int(nullable: false),
                        PostContent = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Movie", t => t.ID)
                .Index(t => t.ID);
            
            CreateStoredProcedure(
                "dbo.Movie_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Genre = p.String(maxLength: 50),
                        Year = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Movie]([Name], [Genre], [Year])
                      VALUES (@Name, @Genre, @Year)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Movie]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[Movie] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Movie_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Genre = p.String(maxLength: 50),
                        Year = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Movie]
                      SET [Name] = @Name, [Genre] = @Genre, [Year] = @Year
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.Movie_Delete",
                p => new
                    {
                        ID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Movie]
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Movie_Delete");
            DropStoredProcedure("dbo.Movie_Update");
            DropStoredProcedure("dbo.Movie_Insert");
            DropForeignKey("dbo.Post", "ID", "dbo.Movie");
            DropForeignKey("dbo.Comment", "Post_ID", "dbo.Post");
            DropIndex("dbo.Post", new[] { "ID" });
            DropIndex("dbo.Comment", new[] { "Post_ID" });
            DropTable("dbo.Post");
            DropTable("dbo.Movie");
            DropTable("dbo.Comment");
        }
    }
}
