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

        //<-- Table Section -->
        public async Task<IActionResult> Table()
        {
            ViewData["Categories"] = await _db.Categories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return View();
        }

        //<-- Create Section -->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM createCategoryVM)
        {
            if(createCategoryVM == null) return BadRequest();
            
            Category category = new()
            {
                Name = createCategoryVM.Name,
            };

            await _db.Categories.AddAsync(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        //<-- Update Section -->
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
            if(updateCategoryVM.Id == null && updateCategoryVM.Id < 0) return BadRequest();
            Category category = await _db.Categories
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == updateCategoryVM.Id);
            if(category == null) return NotFound();

            category.Name = updateCategoryVM.Name;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        //<-- Detail Section -->
        [HttpGet]
        public async Task<IActionResult> Detail(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();

            Category category = await _db.Categories
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (category == null) return NotFound();

            return View(category);
        }

        //<-- Delete Section -->
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
