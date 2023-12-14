using DianaWebApp.Areas.Manage.ViewModels;
using DianaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // <-- Table Section -->
        public async Task<IActionResult> Table()
        {
            ViewData["Products"] = await _db.Products
                .Where(p => !p.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();

            return View();
        }

        // <-- Create Section -->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Color> colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
            List<Material> materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
            List<Size> sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();

            CreateProductVM createProductVM = new()
            {
                Colors = colors,
                Materials = materials,
                Sizes = sizes,
            };
            
            return View(createProductVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {
            if (createProductVM == null) return BadRequest();

            if (!ModelState.IsValid) 
            {
                return View();
            };

            Product newProduct = new()
            {
                Title = createProductVM.Title,
                Description = createProductVM.Description,
                Price = createProductVM.Price,
                CategoryId = int.Parse(createProductVM.CategoryId),
                Images = new List<ProductImage>(),
            };


            foreach (var item in createProductVM.Images)
            {
                if (!item.CheckType("image/"))
                {
                    TempData["Error"] += $"{item.FileName} is not image type!\n";
                    continue;
                }
                if (!item.CheckLength(2097152))
                {
                    TempData["Error"] += $"{item.FileName} size is must lower than 2MB!";
                    continue;
                }

                ProductImage ProductImage = new ProductImage
                {
                    Product = newProduct,
                    ImgUrl = item.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                };

                newProduct.Images.Add(ProductImage);
            }

            // Product Color Section
            if(createProductVM.ColorIds != null)
            {
                foreach (var item in createProductVM.ColorIds)
                {
                    bool existsTag = await _db.Colors.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("ColorIds", "There is no color like this!");
                        createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        return View(createProductVM);
                    }

                    ProductColor productColor = new()
                    {
                        Product = newProduct,
                        ColorId = item,
                    };

                    await _db.ProductColors.AddAsync(productColor); 
                }
            }

            await _db.Products.AddAsync(newProduct);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        // <-- Update Section  -->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Product oldProduct = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(x  => x.Id == Id);
            if(oldProduct == null) return NotFound();



            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(string Model)
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string Model)
        {
            return View();
        }
    }
}
