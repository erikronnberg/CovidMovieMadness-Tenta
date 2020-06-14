using System.ComponentModel.DataAnnotations;

namespace CovidMovieMadness___Tenta.Models
{
    public class Comment
    {
        public int ID { get; set; }
        [Required]
        [StringLength(1000), RegularExpression(@"[A-Z][a-zA-Z0-9]*")]
        public string CommentContent { get; set; }
        [Required]
        [RegularExpression("([0-9]+)")]
        public int UserRating { get; set; }
    }
}