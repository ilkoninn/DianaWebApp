using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class ProductImage : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
