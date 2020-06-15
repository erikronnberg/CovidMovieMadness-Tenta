using CovidMovieMadness___Tenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public int ReviewRating { get; set; }
        public string PostContent { get; set; }

        public virtual List<Comment> Comment { get; set; }
    }
}