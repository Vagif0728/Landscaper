using Landsacper.Areas.Admin.ViewModels;
using Landsacper.DAL;
using Landsacper.Models;
using Landsacper.Utilities.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Landsacper.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.Services.ToListAsync();

            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateSeviceVM serviceVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!serviceVM.Photo.FileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi uygun deyil");
                return View();
            }

            if (!serviceVM.Photo.FileSize(5 * 1024))
            {
                ModelState.AddModelError("Photo", "File olcusu boyukdu");
                return View();
            }

            string fileName = await serviceVM.Photo.Create(_env.WebRootPath, "img", "services");

            Service service = new Service
            {
                Name = serviceVM.Name,
                Description = serviceVM.Description,
                Image = fileName
            };

            await _context.Services.AddRangeAsync(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            UpdateServiceVM serviceVM = new UpdateServiceVM
            {
                Name = service.Name,
                Description = service.Description,
                Image = service.Image
            };

            return View(serviceVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateServiceVM serviceVM)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceVM);
            }

           
            

            Service existed = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

            if (existed == null) return NotFound();
            if (serviceVM.Photo is not null)
            {
                if (!serviceVM.Photo.FileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View(serviceVM);
                }

                if (!serviceVM.Photo.FileSize(5 * 1024))
                {
                    ModelState.AddModelError("Photo", "File olcusu boyukdu");
                    return View(serviceVM);
                }

                string newImage = await serviceVM.Photo.Create(_env.WebRootPath, "img", "services");
                existed.Image.Delete(_env.WebRootPath, "img", "services");
                existed.Image = newImage;

            }


            existed.Name = serviceVM.Name;
            existed.Description = serviceVM.Description;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            service.Image.Delete(_env.WebRootPath, "img", "services");

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
