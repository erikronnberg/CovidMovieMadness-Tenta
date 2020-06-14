namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "CommentContent", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Movie", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Movie", "Genre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Post", "PostContent", c => c.String(nullable: false, maxLength: 1000));
            AlterStoredProcedure(
                "dbo.Movie_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Genre = p.String(maxLength: 50),
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
            
            AlterStoredProcedure(
                "dbo.Movie_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Genre = p.String(maxLength: 50),
                        Year = p.Int(),
                        Post_ID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Movie]
                      SET [Name] = @Name, [Genre] = @Genre, [Year] = @Year, [Post_ID] = @Post_ID
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Post", "PostContent", c => c.String());
            AlterColumn("dbo.Movie", "Genre", c => c.String());
            AlterColumn("dbo.Movie", "Name", c => c.String());
            AlterColumn("dbo.Comment", "CommentContent", c => c.String());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
