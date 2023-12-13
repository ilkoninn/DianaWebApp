using DianaWebApp.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Table()
        {
            ViewData["Categories"] = await _db.Categories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM createCategoryVM)
        {
            Category category = new()
            {
                Name = createCategoryVM.Name,
            };

            await _db.Categories.AddAsync(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();

            Category category = await _db.Categories
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if(category == null) return NotFound();

            UpdateCategoryVM updateCategoryVM = new()
            {
                Id = Id,
                Name = category.Name,
            };

            return View(updateCategoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVM updateCategoryVM)
        {
            Category category = await _db.Categories
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == updateCategoryVM.Id);

            if(category == null) return NotFound();

            category.Name = updateCategoryVM.Name;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();

            Category category = await _db.Categories
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (category == null) return NotFound();

            UpdateCategoryVM updateCategoryVM = new()
            {
                Id = Id,
                Name = category.Name,
            };

            return View(updateCategoryVM);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Category category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (category == null) return NotFound();

            category.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }
    }
}
