using System.ComponentModel.DataAnnotations;
using blogApp.Data;

namespace blogApp.Models
{
	public class PostCreateModel
	{
		[Required(ErrorMessage = "Title is required.")]
		public string? Title { get; set; }

		[Required(ErrorMessage = "URL is required.")]
		[RegularExpression(@"^[a-zA-Z0-9\-]*$", ErrorMessage = "Invalid URL format. Only alphanumeric characters and hyphens are allowed.")]
		public string? Url { get; set; }

		[Required(ErrorMessage = "Content is required.")]
		public string? Content { get; set; }



	}
}
