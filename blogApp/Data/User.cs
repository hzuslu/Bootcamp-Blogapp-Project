

namespace blogApp.Data
{
	public class User
	{
		public int UserId { get; set; }

		public string? UserName { get; set; }

		public string? UserFirstName { get; set; }

		public string? UserSurname { get; set; }

		public string? Password { get; set; }

		public string? Phone { get; set; }

		public string? Image { get; set; }

		public string? Email { get; set; }

		public string? Address { get; set; }

		public string? Country { get; set; }

		public string? City { get; set; }
		public int? ZipCode { get; set; }
		public string? Education { get; set; }

		public DateTime RegisterDate { get; set; }

		public List<Comment> Comments { get; set; } = new List<Comment>();

		public List<Post> Post { get; set; } = new List<Post>();
		public List<Like> Likes { get; set; } = new List<Like>();

		public string UserFullName => $"{UserFirstName} {UserSurname}";

		public List<Event> Events { get; set; } = new List<Event>();


	}
}
