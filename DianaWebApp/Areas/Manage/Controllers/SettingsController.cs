using DianaWebApp.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]

    public class SettingsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SettingsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Detail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            Dictionary<string, string> settings = await _db.Settings
                .Where(x => !x.IsDeleted)
                .ToDictionaryAsync(k => k.Key, v => v.Value);
            if (settings == null) return RedirectToAction(nameof(Update));


            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Dictionary<string, string> settings, IFormFile? Logo)
        {
            foreach (var item in settings)
            {
                var settingItem = await _db.Settings
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync(k => k.Key == item.Key);

                if (item.Key != "Logo")
                {
                    if (settingItem == null)
                    {
                        var newSettingItem = new Settings()
                        {
                            Key = item.Key,
                            Value = item.Value,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                        };

                        await _db.Settings.AddAsync(newSettingItem);
                    }
                    else
                    {
                        settingItem.Value = item.Value;
                        settingItem.CreatedDate = settingItem.CreatedDate;
                        settingItem.UpdatedDate = DateTime.Now;
                    }
                }
                else
                {
                    if (Logo != null)
                    {
                        if (!Logo.CheckType("image/"))
                        {
                            ModelState.AddModelError("Logo", "Logo type is not image");
                        }
                        if (!Logo.CheckLength(2097152))
                        {
                            ModelState.AddModelError("Logo", "Logo image size must be 2MB");
                        }

                        if (!ModelState.IsValid)
                        {
                            settings["Logo"] = settingItem.Value;
                            return View(settings);
                        }
                    }
                    else
                    {
                        continue;
                    }

                    FileManager.Delete(settingItem.Value, _env.WebRootPath, @"\Upload\LogoImage\");
                    settingItem.Value = Logo.Upload(_env.WebRootPath, @"\Upload\LogoImage\");

                }

            }


            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Detail));
        }

    }
}
