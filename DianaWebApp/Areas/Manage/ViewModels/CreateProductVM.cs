namespace DianaWebApp.Areas.Manage.ViewModels
{
    public class CreateProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<Color>? Colors { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<int>? MaterialIds { get; set; }
        public List<Material>? Materials { get; set; }
    }
}
