using System.Security.Claims;
using System.Security.Policy;
using blogApp.Data;
using blogApp.Datacontext;
using blogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
	public class EventController : Controller
	{


		private readonly DataContext _context;

		public EventController(DataContext dataContext)
		{
			_context = dataContext;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> List(string? url)
		{

			var userName = User.FindFirst(ClaimTypes.Name)!.Value;
			IQueryable<Event> eventsQuery = _context.Events
												.Include(e => e.User)
												.Include(e => e.Category)
												.Include(e => e.Users);

			if (url != null)
				eventsQuery = eventsQuery.Where(e => e.User.UserName == userName);


			var events = await eventsQuery.ToListAsync();
			return View(events);
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{

			if (id == null)
				return NotFound();

			var eve = await _context.Events.FirstOrDefaultAsync(u => u.EventId == id);

			if (eve == null)
				return NotFound();

			_context.Remove(eve);
			await _context.SaveChangesAsync();
			return RedirectToAction("List");

		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EventCreate()
		{

			var categories = await _context.Categories.ToListAsync();
			ViewBag.Categories = new SelectList(categories, "CategoryId", "Text");
			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EventCreate(EventCreateModel eventCreateModel)
		{

			if (ModelState.IsValid)
			{

				var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

				var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

				if (user == null)
					return NotFound();

				var eve = new Event()
				{
					EventTitle = eventCreateModel.EventTitle,
					Location = eventCreateModel.Location,
					StartTime = eventCreateModel.StartTime,
					CategoryId = eventCreateModel.CategoryId,
					User = user
				};

				_context.Add(eve);
				await _context.SaveChangesAsync();
				return RedirectToAction("List");

			}

			var categories = await _context.Categories.ToListAsync();
			ViewBag.Categories = new SelectList(categories, "CategoryId", "Text");
			return View(eventCreateModel);
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{

			if (id == null)
				return NotFound();

			Event? eve = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);

			if (eve == null)
				return NotFound();

			EventCreateModel eventCreateModel = new EventCreateModel()
			{
				EventTitle = eve.EventTitle,
				Location = eve.Location,
				CategoryId = eve.CategoryId,
				StartTime = eve.StartTime,
				EventId = eve.EventId

			};

			var categories = await _context.Categories.ToListAsync();
			ViewBag.Categories = new SelectList(categories, "CategoryId", "Text");
			return View(eventCreateModel);

		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(EventCreateModel eventCreateModel)
		{
			var role = User.FindFirst(ClaimTypes.Role);

			if (ModelState.IsValid)
			{
				var eve = await _context.Events.FindAsync(eventCreateModel.EventId);



				if (eve == null)
				{
					return NotFound();
				}

				eve.EventTitle = eventCreateModel.EventTitle;
				eve.CategoryId = eventCreateModel.CategoryId;
				eve.StartTime = eventCreateModel.StartTime;
				eve.Location = eventCreateModel.Location;

				_context.Update(eve);
				await _context.SaveChangesAsync();
				var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == eve.UserId);

				if (role == null)
					return RedirectToAction("List", new { url = user!.UserName });
				return RedirectToAction("List");
			}

			var categories = await _context.Categories.ToListAsync();
			ViewBag.Categories = new SelectList(categories, "CategoryId", "Text");
			return View(eventCreateModel);
		}


		[HttpGet]
		[Authorize]
		public IActionResult Calendar()
		{
			return View(_context.Events.ToList());
		}
	}
}
