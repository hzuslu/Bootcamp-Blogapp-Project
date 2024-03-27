

using blogApp.Datacontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.ViewComponents
{
    public class Calendar : ViewComponent
    {
        private readonly DataContext _context;

        public Calendar(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IViewComponentResult Invoke()
        {


            return View();
        }


    }
}
