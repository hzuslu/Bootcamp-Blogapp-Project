namespace blogApp.Data
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string? Text { get; set; }
		public List<Event> Events { get; set; } = new List<Event>();

	}
}
