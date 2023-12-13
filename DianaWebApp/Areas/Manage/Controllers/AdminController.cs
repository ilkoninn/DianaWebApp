using Microsoft.AspNetCore.Mvc;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AdminController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
