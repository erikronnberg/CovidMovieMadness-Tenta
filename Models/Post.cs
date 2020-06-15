﻿using System;
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
        [StringLength(1000), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-]*$")]
        [DataType(DataType.MultilineText)]
        public string PostContent { get; set; }
        [Required]
        [StringLength(1000), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s\w-]*$")]
        public string PostTitle { get; set; }
        public DateTime PostDate { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual List<Comment> Comment { get; set; }
    }
}