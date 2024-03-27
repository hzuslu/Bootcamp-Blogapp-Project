using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Username is required.")]
		public string? UserName { get; set; }

		[Required(ErrorMessage = "User surname is required.")]
		public string? UserSurname { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Password Confirm is required.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string? PasswordConfirm { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		[Phone(ErrorMessage = "Invalid phone number format.")]
		public string? Phone { get; set; }

		[Required(ErrorMessage = "Email address is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address format.")]
		public string? Email { get; set; }
	}
}
