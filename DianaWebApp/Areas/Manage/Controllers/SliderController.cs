using Microsoft.AspNetCore.Mvc;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        public async Task<IActionResult> Table()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string Model)
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Update()
        {
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
