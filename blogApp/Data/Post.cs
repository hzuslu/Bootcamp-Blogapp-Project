using System;
using System.Collections.Generic;

namespace blogApp.Data
{
    public class Post
    {
        public int PostId { get; set; }

        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; } = null;
        public DateTime PostTime { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Like> Likes { get; set; } = new List<Like>();
    }
}
