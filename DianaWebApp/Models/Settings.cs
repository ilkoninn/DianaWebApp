using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Settings : BaseAuditableEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
