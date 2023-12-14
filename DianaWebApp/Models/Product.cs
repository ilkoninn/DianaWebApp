using DianaWebApp.Models.Entity;
using System.Drawing;

namespace DianaWebApp.Models
{
    public class Product : BaseAuditableEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<ProductMaterial> ProductMaterials { get; set; }
    }
}
