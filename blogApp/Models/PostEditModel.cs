using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
    public class PostEditModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "URL is required")]
        public string? Url { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }

        public bool IsActive { get; set; }

        public string? Image { get; set; }

    }
}
