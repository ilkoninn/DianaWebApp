using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Material : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<ProductMaterial> ProductMaterials { get; set; }
    }
}
