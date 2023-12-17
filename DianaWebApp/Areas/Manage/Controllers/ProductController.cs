using DianaWebApp.Areas.Manage.ViewModels;
using DianaWebApp.Models;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Runtime.InteropServices;
using static DianaWebApp.Areas.Manage.ViewModels.UpdateProductVM;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]

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

            ViewData["Categories"] = await _db.Categories.ToListAsync();
            ViewData["Colors"] = await _db.Colors.ToListAsync();
            ViewData["Materials"] = await _db.Materials.ToListAsync();
            ViewData["Sizes"] = await _db.Sizes.ToListAsync();

            return View();
        }

        // <-- Create Section -->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Color> colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
            List<Material> materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
            List<Size> sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
            List<Category> categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

            CreateProductVM createProductVM = new()
            {
                Categories = categories,
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

            // Check Product Section
            var existsSameTitle = await _db.Products
                .Where(x => !x.IsDeleted && x.Title == createProductVM.Title )
                .FirstOrDefaultAsync() != null;
            var existsSameDescription = await _db.Products
                .Where(x => !x.IsDeleted && x.Description == createProductVM.Description)
                .FirstOrDefaultAsync() != null;

            if (existsSameTitle)
            {
                ModelState.AddModelError("Title", "There is a same title product in Table!");
            }
            if (existsSameDescription)
            {
                ModelState.AddModelError("Description", "There is a same description product in Table!");
            }


            if (!ModelState.IsValid)
            {
                createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();
                
                return View(createProductVM);
            };

            Product newProduct = new()
            {
                Title = createProductVM.Title,
                Description = createProductVM.Description,
                Price = createProductVM.Price,
                CategoryId = int.Parse(createProductVM.CategoryId),
                Images = new List<ProductImage>(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            // Product Image Section
            if (createProductVM.Images != null)
            {
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
            }
            else
            {
                ModelState.AddModelError("Images", "You must be upload a photo!");
                createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                createProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                return View(createProductVM);
            }

            // Product Color Section
            if (createProductVM.ColorIds != null)
            {
                foreach (var item in createProductVM.ColorIds)
                {
                    bool existsColor = await _db.Colors.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsColor)
                    {
                        ModelState.AddModelError("ColorIds", "There is no color like this!");
                        createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(createProductVM);
                    }

                    ProductColor newProductColor = new()
                    {
                        Product = newProduct,
                        ColorId = item,
                    };

                    await _db.ProductColors.AddAsync(newProductColor);
                }
            }

            // Product Size Section
            if (createProductVM.SizeIds != null)
            {
                foreach (var item in createProductVM.SizeIds)
                {
                    bool existsSize = await _db.Sizes.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsSize)
                    {
                        ModelState.AddModelError("SizeIds", "There is no size like this!");
                        createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(createProductVM);
                    }

                    ProductSize newProductSize = new()
                    {
                        Product = newProduct,
                        SizeId = item,
                    };

                    await _db.ProductSizes.AddAsync(newProductSize);
                }
            }

            // Product Material Section
            if (createProductVM.MaterialIds != null)
            {
                foreach (var item in createProductVM.MaterialIds)
                {
                    bool existsMaterial = await _db.Materials.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsMaterial)
                    {
                        ModelState.AddModelError("MaterialIds", "There is no material like this!");
                        createProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        createProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(createProductVM);
                    }

                    ProductMaterial newProductMaterial = new()
                    {
                        Product = newProduct,
                        MaterialId = item,
                    };

                    await _db.ProductMaterials.AddAsync(newProductMaterial);
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
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (oldProduct == null) return NotFound();

            UpdateProductVM updateProductVM = new()
            {
                Title = oldProduct.Title,
                Description = oldProduct.Description,
                Price = oldProduct.Price,
                CategoryId = $"{oldProduct.CategoryId}",
                Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync(),
                productImageVMs = new List<ProductImageVM>(),
                Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync(),
                ColorIds = oldProduct.ProductColors.Select(x => x.ColorId).ToList(),
                Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync(),
                MaterialIds = oldProduct.ProductMaterials.Select(x => x.MaterialId).ToList(),
                Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync(),
                SizeIds = oldProduct.ProductSizes.Select(x => x.SizeId).ToList(),
            };

            foreach (var item in oldProduct.Images)
            {
                ProductImageVM productImageVM = new()
                {
                    Id = item.Id,
                    ImgUrl = item.ImgUrl,
                };

                updateProductVM.productImageVMs.Add(productImageVM);
            }

            return View(updateProductVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateProductVM)
        {
            if (updateProductVM.Id == null && updateProductVM.Id < 0) return BadRequest();
            Product oldProduct = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(x => x.Id == updateProductVM.Id);
            if (oldProduct == null) return NotFound();

            // Check Product Section
            var existsSameTitle = await _db.Products
                .Where(x => !x.IsDeleted && x.Title == updateProductVM.Title && x.Id != updateProductVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsSameDescription = await _db.Products
                .Where(x => !x.IsDeleted && x.Description == updateProductVM.Description && x.Id != updateProductVM.Id)
                .FirstOrDefaultAsync() != null;

            if (existsSameTitle)
            {
                ModelState.AddModelError("Title", "There is a same title Product in Table!");
            }
            if (existsSameDescription)
            {
                ModelState.AddModelError("Description", "There is a same description Product in Table!");
            }

            if (!ModelState.IsValid)
            {
                updateProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.productImageVMs = new();

                foreach (var item in oldProduct.Images)
                {
                    ProductImageVM productImageVM = new()
                    {
                        Id = item.Id,
                        ImgUrl = item.ImgUrl,
                    };

                    updateProductVM.productImageVMs.Add(productImageVM);
                }

                return View(updateProductVM);
            }

            oldProduct.Title = updateProductVM.Title;
            oldProduct.Description = updateProductVM.Description;
            oldProduct.Price = updateProductVM.Price;
            oldProduct.UpdatedDate = DateTime.Now;
            oldProduct.CreatedDate = oldProduct.CreatedDate;
            oldProduct.CategoryId = int.Parse(updateProductVM.CategoryId);

            oldProduct.ProductColors.Clear();
            oldProduct.ProductMaterials.Clear();
            oldProduct.ProductSizes.Clear();

            // Product Color Section
            if (updateProductVM.ColorIds != null)
            {
                foreach (var item in updateProductVM.ColorIds)
                {
                    bool existsColor = await _db.Colors.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsColor)
                    {
                        ModelState.AddModelError("ColorIds", "There is no color like this!");
                        updateProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(updateProductVM);
                    }

                    ProductColor newProductColor = new()
                    {
                        Product = oldProduct,
                        ColorId = item,
                    };

                    await _db.ProductColors.AddAsync(newProductColor);
                }
            }

            // Product Size Section
            if (updateProductVM.SizeIds != null)
            {
                foreach (var item in updateProductVM.SizeIds)
                {
                    bool existsSize = await _db.Sizes.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsSize)
                    {
                        ModelState.AddModelError("SizeIds", "There is no size like this!");
                        updateProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(updateProductVM);
                    }

                    ProductSize newProductSize = new()
                    {
                        Product = oldProduct,
                        SizeId = item,
                    };

                    await _db.ProductSizes.AddAsync(newProductSize);
                }
            }

            // Product Material Section
            if (updateProductVM.MaterialIds != null)
            {
                foreach (var item in updateProductVM.MaterialIds)
                {
                    bool existsMaterial = await _db.Materials.Where(x => !x.IsDeleted).AnyAsync(c => c.Id == item);

                    if (!existsMaterial)
                    {
                        ModelState.AddModelError("MaterialIds", "There is no material like this!");
                        updateProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                        updateProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();

                        return View(updateProductVM);
                    }

                    ProductMaterial newProductMaterial = new()
                    {
                        Product = oldProduct,
                        MaterialId = item,
                    };

                    await _db.ProductMaterials.AddAsync(newProductMaterial);
                }
            }


            // Remove Images Section
            if (updateProductVM.ImageIds == null)
            {
                oldProduct.Images.Clear();
            }
            else
            {
                List<ProductImage> removeList = oldProduct.Images.Where(pt => !updateProductVM.ImageIds.Contains(pt.Id)).ToList();

                if (removeList.Count > 0)
                {
                    foreach (var item in removeList)
                    {
                        oldProduct.Images.Remove(item);
                        item.ImgUrl.Delete(_env.WebRootPath, @"\Upload\ProductImages\");
                    }
                }
            }

            // Product Image Section
            if (updateProductVM.Images != null)
            {
                foreach (var item in updateProductVM.Images)
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
                        Product = oldProduct,
                        ImgUrl = item.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                    };

                    oldProduct.Images.Add(ProductImage);
                }
            }

            if (oldProduct.Images.Count == 0)
            {
                ModelState.AddModelError("Images", "You must be upload a photo!");
                updateProductVM.Colors = await _db.Colors.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Sizes = await _db.Sizes.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Materials = await _db.Materials.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.Categories = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();
                updateProductVM.productImageVMs = new();

                return View(updateProductVM);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Product oldProduct = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(x => x.Category)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (oldProduct == null) return NotFound();

            oldProduct.Colors = oldProduct.ProductColors.Select(x => x.Color).ToList();
            oldProduct.Sizes = oldProduct.ProductSizes.Select(x => x.Size).ToList();
            oldProduct.Materials = oldProduct.ProductMaterials.Select(x => x.Material).ToList();

            return View(oldProduct);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Product oldProduct = await _db.Products
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (oldProduct == null) return NotFound();

            oldProduct.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }
    }
}
