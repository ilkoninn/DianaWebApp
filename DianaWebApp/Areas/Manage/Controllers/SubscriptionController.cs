using Microsoft.AspNetCore.Mvc;

namespace DianaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        private readonly AppDbContext _db;
        public SubscriptionController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Table()
        {
            List<Subscription> subscriptions = await _db.Subscriptions
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return View(subscriptions);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id < 0 && Id == null) return BadRequest();
            Subscription subscription = await _db.Subscriptions
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if(subscription == null) return NotFound();

            subscription.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

    }
}
