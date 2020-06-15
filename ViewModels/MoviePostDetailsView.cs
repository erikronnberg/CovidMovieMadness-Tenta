using CovidMovieMadness___Tenta.Models;
using System;
using System.Collections.Generic;

namespace CovidMovieMadness___Tenta.ViewModels
{
    public class MoviePostDetailsView
    {
        //Movie
        public int MovieID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public DateTime Year { get; set; }

        //Post
        public int PostID { get; set; }
        public int PostRating { get; set; }
        public string PostContent { get; set; }
        public string PostTitle { get; set; }

        public virtual List<Comment> Comment { get; set; }
    }
}