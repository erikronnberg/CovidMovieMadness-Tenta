using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CovidMovieMadness___Tenta.Models
{
    public class Post
    {
        public int ID { get; set; }
        [Required]
        [Range(0,10)]
        public int ReviewRating { get; set; }
        [Required]
        [StringLength(1000), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-]*$")]
        public string PostContent { get; set; }

        public virtual Movie Movie { get; set; }
        public ICollection<Movie> Comment { get; internal set; }
    }
}