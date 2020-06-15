using System.ComponentModel.DataAnnotations;

namespace CovidMovieMadness___Tenta.Models
{
    public class Comment
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        public string CommentContent { get; set; }
        [Required]
        [Range(0, 10)]
        public int UserRating { get; set; }
    }
}