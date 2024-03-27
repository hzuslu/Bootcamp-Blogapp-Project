using System.Security.Claims;
using blogApp.Data;
using blogApp.Datacontext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
	public class TagController : Controller
	{

		private readonly DataContext _context;

		public TagController(DataContext dataContext)
		{
			_context = dataContext;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> List()
		{
			if (User.FindFirst(ClaimTypes.Role) == null)
				return RedirectToAction("Index", "Post");

			var tags = await _context.Tags.ToListAsync();
			return View(tags);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddTag(string? text, string? url)
		{
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(url))
			{
				return NotFound();
			}

			var tag = new Tag
			{
				Text = text,
				Url = url
			};

			_context.Add(tag);
			await _context.SaveChangesAsync();


			return Json(new { id = tag.TagId, text = tag.Text , url = tag.Url });
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteTag(int? tagId)
		{
			if (tagId == null)
				return NotFound();

			var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagId == tagId);

			if (tag == null)
				return NotFound();

			_context.Remove(tag);
			await _context.SaveChangesAsync();
			return Json(new { id = tag.TagId, success = true });
		}

	}
}
