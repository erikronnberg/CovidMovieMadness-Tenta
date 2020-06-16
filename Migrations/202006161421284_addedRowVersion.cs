namespace CovidMovieMadness___Tenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterStoredProcedure(
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
                      
                      SELECT t0.[ID], t0.[RowVersion]
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
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Movie]
                      SET [Name] = @Name, [Genre] = @Genre, [Year] = @Year
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Movie] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            AlterStoredProcedure(
                "dbo.Movie_Delete",
                p => new
                    {
                        ID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Movie]
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "RowVersion");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
