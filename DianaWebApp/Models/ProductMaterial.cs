using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class ProductMaterial : BaseAuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}
