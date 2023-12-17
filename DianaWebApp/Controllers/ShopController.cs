using Microsoft.AspNetCore.Mvc;

namespace DianaWebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _db;

        public ShopController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Single(int Id)
        {
            if (Id == null && Id < 0) return BadRequest();
            Product singleProduct = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (singleProduct == null) return NotFound();



            return View(singleProduct);
        }

    }
}
