using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Category : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get;}
    }
}
