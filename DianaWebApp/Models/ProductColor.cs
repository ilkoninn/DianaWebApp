using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class ProductColor : BaseAuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
