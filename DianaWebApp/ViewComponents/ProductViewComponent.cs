namespace DianaWebApp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public ProductViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int key = 0)
        {
            List<Product> products;
            ViewBag.IsChange1 = false;
            ViewBag.IsChange2 = false;
            
            
            switch (key)
            {
                case 1:
                    products = await _db.Products
                .Where(x => !x.IsDeleted && x.Category.Name == "Cat 1")
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;
                case 2:
                    products = await _db.Products
                .Where(x => !x.IsDeleted && x.Category.Name == "Cat 2")
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;
                case 3:
                    products = await _db.Products
                .Where(x => !x.IsDeleted && x.Category.Name == "Cat 3")
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;
                case 4:
                    ViewBag.IsChange1 = true;
                    products = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;
                case 5:
                    ViewBag.IsChange2 = true;
                    products = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;
                default:
                    products = await _db.Products
                .Where(x => !x.IsDeleted)
                .Include(pi => pi.Images)
                .Include(pcl => pcl.ProductColors)
                .ThenInclude(cl => cl.Color)
                .Include(pml => pml.ProductMaterials)
                .ThenInclude(ml => ml.Material)
                .Include(ps => ps.ProductSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();
                    break;

            }

            return View(products);
        }
    }
}
