using System.Collections.Generic;

namespace CovidMovieMadness___Tenta.Models
{
    public class Post
    {
        public int ID { get; set; }
        public int ReviewRating { get; set; }
        public string PostContent { get; set; }

        public virtual Movie Movie { get; set; }
        public ICollection<Movie> Comment { get; internal set; }
    }
}