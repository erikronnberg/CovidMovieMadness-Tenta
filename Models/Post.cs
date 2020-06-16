using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidMovieMadness___Tenta.Models
{
    public class Post
    {
        [ForeignKey("Movie")]
        public int ID { get; set; }
        [Required]
        [Range(0, 10)]
        public int PostRating { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        [DataType(DataType.MultilineText)]
        public string PostContent { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        public string PostTitle { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual List<Comment> Comment { get; set; }
    }
}