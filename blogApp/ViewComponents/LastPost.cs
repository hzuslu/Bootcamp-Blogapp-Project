

using blogApp.Datacontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.ViewComponents
{
    public class LastPost : ViewComponent
    {
        private readonly DataContext _context;

        public LastPost(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IViewComponentResult Invoke()
        {

            var post = _context.Posts
                        .Include(p => p.User)
                        .Where(p => p.IsActive == true)
                        .ToList()
                        .Last();
            return View(post);
        }


    }
}
