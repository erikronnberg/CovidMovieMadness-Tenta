using System;
using System.ComponentModel.DataAnnotations;

namespace CovidMovieMadness___Tenta.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Input value must be between {2} and {1} characters"), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-.!,;:]*$", ErrorMessage = "First character uppercase and only alphanumerical and punctuation characters")]
        public string Genre { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Post Post { get; set; }
    }
}