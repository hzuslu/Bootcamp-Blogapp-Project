namespace blogApp.Data
{
	public class Event
	{
		public int? EventId { get; set; }

		public string? EventTitle { get; set; }

		public string? Location { get; set; }

		public DateTime StartTime { get; set; }

		public int UserId { get; set; }


		public User User { get; set; } = null!;

		public int CategoryId { get; set; }
		public Category Category { get; set; } = null!;

		public List<User> Users { get; set; } = new List<User>();
	}
}
