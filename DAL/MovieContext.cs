using CovidMovieMadness___Tenta.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CovidMovieMadness___Tenta.DAL
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Movie>()
            //    .HasOptional(c => c.Post).WithMany(c => (System.Collections.Generic.ICollection<Movie>)c.Comment).WillCascadeOnDelete();
            modelBuilder.Entity<Movie>()
                .HasOptional(c => c.Post);
            modelBuilder.Entity<Movie>().MapToStoredProcedures();
        }
    }
}