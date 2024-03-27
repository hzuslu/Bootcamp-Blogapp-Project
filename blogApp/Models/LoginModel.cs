using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
	public class LoginModel
	{

		[Required(ErrorMessage = "*")]
		[EmailAddress(ErrorMessage = "Invalid email address format")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "*")]
		public string? Password { get; set; }

		public bool Remember { get; set;}

	}
}
