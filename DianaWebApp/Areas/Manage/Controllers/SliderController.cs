
using DianaWebApp.Areas.Manage.ViewModels;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Table()
        {
            ViewData["Sliders"] = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return View();
        }


        // <-- Create Section -->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM createSliderVM)
        {
            if (createSliderVM == null) return BadRequest();

            // Check Slider Section
            var existsSameTitle =  await _db.Sliders
                .Where(x => !x.IsDeleted && x.Title == createSliderVM.Title)
                .FirstOrDefaultAsync() != null;
            var existsSameDescription = await _db.Sliders
                .Where(x => !x.IsDeleted && x.Description == createSliderVM.Description)
                .FirstOrDefaultAsync() != null;

            if(existsSameTitle)
            {
                ModelState.AddModelError("Title", "There is a same title slider in Table!");
            }
            if (existsSameDescription)
            {
                ModelState.AddModelError("Description", "There is a same description slider in Table!");
            }
            if(createSliderVM.File != null)
            {
                if (!createSliderVM.File.CheckType("image/"))
                {
                    ModelState.AddModelError("File", "File type is not image");
                }
                if (!createSliderVM.File.CheckLength(2097152))
                {
                    ModelState.AddModelError("File", "File size must be 2MB");
                }
            }
            else
            {
                ModelState.AddModelError("File", "You must be upload a photo(1920x1080)!");
            }

            if (!ModelState.IsValid)
            {
                return View(createSliderVM);
            }

            Slider newSlider = new() 
            {
                Title = createSliderVM.Title,
                Description = createSliderVM.Description,
                ImgUrl = createSliderVM.File.Upload(_env.WebRootPath, @"\Upload\SliderImages\"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _db.Sliders.AddAsync(newSlider);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        // <-- Update Section -->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Slider oldSlider = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if(oldSlider == null) return NotFound();

            UpdateSliderVM updateSliderVM = new()
            {
                Title = oldSlider.Title,
                Description = oldSlider.Description,
            };

            return View(updateSliderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSliderVM updateSliderVM)
        {
            if(updateSliderVM.Id == null && updateSliderVM.Id < 0) return BadRequest();
            Slider oldSlider = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == updateSliderVM.Id);
            if(oldSlider == null) return NotFound();

            // Check Slider Section
            var existsSameTitle = await _db.Sliders
                .Where(x => !x.IsDeleted && x.Title == updateSliderVM.Title && x.Id != updateSliderVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsSameDescription = await _db.Sliders
                .Where(x => !x.IsDeleted && x.Description == updateSliderVM.Description && x.Id != updateSliderVM.Id)
                .FirstOrDefaultAsync() != null;

            if (existsSameTitle)
            {
                ModelState.AddModelError("Title", "There is a same title slider in Table!");
            }
            if (existsSameDescription)
            {
                ModelState.AddModelError("Description", "There is a same description slider in Table!");
            }
            if (updateSliderVM.File != null)
            {
                if (!updateSliderVM.File.CheckType("image/"))
                {
                    ModelState.AddModelError("File", "File type is not image");
                }
                if (!updateSliderVM.File.CheckLength(2097152))
                {
                    ModelState.AddModelError("File", "File size must be 2MB");
                }
            }
            else
            {
                ModelState.AddModelError("File", "You must be upload a photo(1920x1080)!");
            }

            if (!ModelState.IsValid)
            {
                return View(updateSliderVM);
            }

            oldSlider.Title = updateSliderVM.Title;
            oldSlider.Description = updateSliderVM.Description;
            oldSlider.ImgUrl = updateSliderVM.File.Upload(_env.WebRootPath, @"\Upload\SliderImages\");
            oldSlider.CreatedDate = oldSlider.CreatedDate;
            oldSlider.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        // <-- Detail Section -->
        [HttpGet]
        public async Task<IActionResult> Detail(int Id)
        {
            if(Id < 0 && Id == null) return BadRequest();
            Slider oldSlider = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if(oldSlider == null) return NotFound(); 

            return View(oldSlider);
        }


        // <-- Delete Section -->
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if(Id < 0 && Id == null) return BadRequest();
            Slider oldSlider = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if( oldSlider == null) return NotFound();

            oldSlider.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }
    }
}
