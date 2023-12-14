using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Color : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
