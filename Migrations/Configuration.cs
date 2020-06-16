namespace CovidMovieMadness___Tenta.Migrations
{
    using CovidMovieMadness___Tenta.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CovidMovieMadness___Tenta.DAL.MovieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CovidMovieMadness___Tenta.DAL.MovieContext context)
        {
            var movie1 = new Movie() { ID = 1, Name = "The Terminator", Genre = "Action", Year = new DateTime(1984, 10, 26) };
            var movie2 = new Movie() { ID = 2, Name = "The Terminator 2", Genre = "Action", Year = new DateTime(1991, 9, 13) };
            var movie3 = new Movie() { ID = 3, Name = "My Little Pony", Genre = "Adventure", Year = new DateTime(2017, 10, 20) };


            var comment1 = new Comment() { ID = 1, Username = "ErikCool2000", UserRating = 2, CommentContent = "I don't like Arnold cause I am a zoomer" };
            var comment2 = new Comment() { ID = 2, Username = "ErikinteCool2000", UserRating = 5, CommentContent = "I love Arnold" };
            var comment3 = new Comment() { ID = 3, Username = "AdamG", UserRating = 5, CommentContent = "I love Terminator 2" };
            var comment4 = new Comment() { ID = 4, Username = "JonathanZoomer", UserRating = 1, CommentContent = "I hate Terminator 2" };
            var comment5 = new Comment() { ID = 5, Username = "ErikPonyLover2000", UserRating = 5, CommentContent = "I wish I could give more than 5 stars" };
            var comment6 = new Comment() { ID = 6, Username = "JonathanPonyHater2000", UserRating = 1, CommentContent = "I wish I could give less than a 1" };

            var post1 = new Post() { Movie = movie1, ID = 1, PostRating = 4, PostTitle = "Best movie ever", PostContent = "This was one of the best movies I have seen", PostDate = DateTime.Now, Comment = new List<Comment>() { comment1, comment2 } };
            var post2 = new Post() { Movie = movie2, ID = 2, PostRating = 2, PostTitle = "Worst movie ever", PostContent = "I don't like Arnold cause I am a zoomer", PostDate = new DateTime(2020, 5, 16), Comment = new List<Comment>() { comment3, comment4 } };
            var post3 = new Post() { Movie = movie3, ID = 3, PostRating = 5, PostTitle = "I like ponies", PostContent = "I really like ponies", PostDate = new DateTime(2020, 06, 16), Comment = new List<Comment>() { comment5, comment6 } };

            context.Movie.AddOrUpdate(movie1, movie2, movie3);
            context.Comment.AddOrUpdate(comment1, comment2, comment3, comment4, comment5, comment6);
            context.Post.AddOrUpdate(post1, post2, post3);
        }
    }
}
