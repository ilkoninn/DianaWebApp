using System.ComponentModel.DataAnnotations.Schema;

namespace DianaWebApp.Areas.Manage.ViewModels
{
    public class UpdateSliderVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
