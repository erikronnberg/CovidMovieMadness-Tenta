using System.ComponentModel.DataAnnotations;

namespace CovidMovieMadness___Tenta.Models
{
    public class Comment
    {

        public int ID { get; set; }
        [Required]
        [StringLength(1000), RegularExpression(@"^[a-zA-Z0-9""'\s\w-]*$")]
        public string Username { get; set; }
        [Required]
        [StringLength(1000), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-]*$")]
        public string CommentContent { get; set; }
        [Required]
        [Range(0,10)]
        public int UserRating { get; set; }
    }
}