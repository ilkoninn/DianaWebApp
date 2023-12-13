using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class ProductSize : BaseAuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
    }
}
