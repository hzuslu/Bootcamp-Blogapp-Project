

using blogApp.Datacontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.ViewComponents
{
    public class Author : ViewComponent
    {
        private readonly DataContext _context;

        public Author(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IViewComponentResult Invoke()
        {


            return View();
        }


    }
}
