namespace DianaWebApp.ViewComponents
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public SliderViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Slider> sliders = await _db.Sliders
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return View(sliders);
        }
    }
}
