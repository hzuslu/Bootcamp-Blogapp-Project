using System.ComponentModel.DataAnnotations;

namespace blogApp.Data
{
	public class Comment
	{
		public int CommentId { get; set; }

		public string? Content { get; set; }
		public DateTime CommentTime { get; set; }

		public int PostId { get; set; }

		public Post Post { get; set; } = null!;

		public int UserId { get; set; }

		public User User { get; set; } = null!;

	}
}
