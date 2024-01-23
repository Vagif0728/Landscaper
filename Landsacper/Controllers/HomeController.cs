using Landsacper.DAL;
using Landsacper.Models;
using Landsacper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landsacper.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
          _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.Services.ToListAsync();
            HomeVM home = new HomeVM
            {
                Services = services
            };

            return View(home);
        }
    }
}
