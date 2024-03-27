using System.Security.Claims;
using blogApp.Data;
using blogApp.Datacontext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
	public class CategoryController : Controller
	{

		private readonly DataContext _context;

		public CategoryController(DataContext dataContext)
		{
			_context = dataContext;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> List()
		{
			if (User.FindFirst(ClaimTypes.Role) == null)
				return RedirectToAction("Index", "Post");

			var categories = await _context.Categories.ToListAsync();
			return View(categories);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddCategory(string? text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return NotFound();
			}

			var category = new Category
			{
				Text = text
			};

			_context.Add(category);
			await _context.SaveChangesAsync();


			return Json(new { id = category.CategoryId, text = category.Text });
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteCategory(int? categoryId)
		{
			if (categoryId == null)
				return NotFound();

			var cat = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

			if (cat == null)
				return NotFound();

			_context.Remove(cat);
			await _context.SaveChangesAsync();
			return Json(new { id = cat.CategoryId, success = true });
		}

	}
}
