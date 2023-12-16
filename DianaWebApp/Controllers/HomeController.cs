

namespace DianaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Sliders"] = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            ViewData["Categories"] = await _db.Categories.ToListAsync();
            ViewData["Emails"] = await _db.Subscriptions.ToListAsync();

            return View();
        }
    }
}
