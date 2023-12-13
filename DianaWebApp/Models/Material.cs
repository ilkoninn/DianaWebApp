using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Material : BaseAuditableEntity
    {
        public string Name { get; set; }
        //public ICollection<Product> Products { get; set; }
    }
}
