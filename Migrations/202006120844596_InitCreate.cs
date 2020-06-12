namespace CovidMovieMadness___Tenta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CommentContent = c.String(),
                    UserRating = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Movie",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Genre = c.String(),
                    Year = c.Int(nullable: false),
                    Post_ID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Post", t => t.Post_ID)
                .Index(t => t.Post_ID);

            CreateTable(
                "dbo.Post",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ReviewRating = c.Int(nullable: false),
                    PostContent = c.String(),
                    Movie_ID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Movie", t => t.Movie_ID)
                .Index(t => t.Movie_ID);

            CreateStoredProcedure(
                "dbo.Movie_Insert",
                p => new
                {
                    Name = p.String(),
                    Genre = p.String(),
                    Year = p.Int(),
                    Post_ID = p.Int(),
                },
                body:
                    @"INSERT [dbo].[Movie]([Name], [Genre], [Year], [Post_ID])
                      VALUES (@Name, @Genre, @Year, @Post_ID)
                      
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
                    Name = p.String(),
                    Genre = p.String(),
                    Year = p.Int(),
                    Post_ID = p.Int(),
                },
                body:
                    @"UPDATE [dbo].[Movie]
                      SET [Name] = @Name, [Genre] = @Genre, [Year] = @Year, [Post_ID] = @Post_ID
                      WHERE ([ID] = @ID)"
            );

            CreateStoredProcedure(
                "dbo.Movie_Delete",
                p => new
                {
                    ID = p.Int(),
                    Post_ID = p.Int(),
                },
                body:
                    @"DELETE [dbo].[Movie]
                      WHERE (([ID] = @ID) AND (([Post_ID] = @Post_ID) OR ([Post_ID] IS NULL AND @Post_ID IS NULL)))"
            );

        }

        public override void Down()
        {
            DropStoredProcedure("dbo.Movie_Delete");
            DropStoredProcedure("dbo.Movie_Update");
            DropStoredProcedure("dbo.Movie_Insert");
            DropForeignKey("dbo.Movie", "Post_ID", "dbo.Post");
            DropForeignKey("dbo.Post", "Movie_ID", "dbo.Movie");
            DropIndex("dbo.Post", new[] { "Movie_ID" });
            DropIndex("dbo.Movie", new[] { "Post_ID" });
            DropTable("dbo.Post");
            DropTable("dbo.Movie");
            DropTable("dbo.Comment");
        }
    }
}
