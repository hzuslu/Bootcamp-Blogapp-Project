using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
	public class UserEditModel
	{
		public string? UserFirstName { get; set; }


		[Required(ErrorMessage = "Last name field is required.")]

		public string? UserSurname { get; set; }

		[Required(ErrorMessage = "Phone number field is required.")]
		public string? Phone { get; set; }

		public string? Image { get; set; }

		[Required(ErrorMessage = "Email field is required.")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string? Email { get; set; }

		public string? Address { get; set; }
		public string? Country { get; set; }
		public string? City { get; set; }
		public int? ZipCode { get; set; }
		public string? Education { get; set; }
	}
}
