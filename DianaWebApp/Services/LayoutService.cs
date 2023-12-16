namespace DianaWebApp.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _db;

        public LayoutService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _db.Settings
                .Where(k => !k.IsDeleted)
                .ToDictionaryAsync(k => k.Key, v => v.Value);

            return settings;
        }
    }
}
