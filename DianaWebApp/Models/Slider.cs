using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Slider : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
