using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models.Account
{
    public class Subscription : BaseAuditableEntity
    {
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
