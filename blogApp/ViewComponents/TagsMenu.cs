
using blogApp.Datacontext;
using Microsoft.AspNetCore.Mvc;

namespace blogApp.ViewComponents
{
	public class TagsMenu : ViewComponent
	{
		private readonly DataContext _context;

		public TagsMenu(DataContext dataContext)
		{
			_context = dataContext;
		}

		public IViewComponentResult Invoke()
		{
			return View(_context.Tags.ToList());
		}

	}
}
