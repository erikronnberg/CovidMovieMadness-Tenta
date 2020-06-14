namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedDataTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "Year", c => c.DateTime(nullable: false));
            AlterStoredProcedure(
                "dbo.Movie_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Genre = p.String(maxLength: 50),
                        Year = p.DateTime(),
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
                        Year = p.DateTime(),
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
            AlterColumn("dbo.Movie", "Year", c => c.Int(nullable: false));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
