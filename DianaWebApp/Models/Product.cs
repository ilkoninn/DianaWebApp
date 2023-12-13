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
        //public ICollection<Color> Colors { get; set; }
        //public ICollection<Size> Sizes { get; set; }
        //public ICollection<Material> Materials { get; set; }
    }
}
