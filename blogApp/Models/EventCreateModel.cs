using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
	public class EventCreateModel
	{
		public int? EventId { get; set; }

		[Required(ErrorMessage = "Event title is required.")]
		[Display(Name = "Event Title")]
		public string? EventTitle { get; set; }

		[Required(ErrorMessage = "Location is required.")]
		public string? Location { get; set; }

		[Required(ErrorMessage = "Start time is required.")]
		[Display(Name = "Start Time")]
		public DateTime StartTime { get; set; }

		[Required(ErrorMessage = "Category ID is required.")]
		[Display(Name = "Category ID")]
		public int CategoryId { get; set; }
	}
}
