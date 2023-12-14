namespace DianaWebApp.Areas.Manage.ViewModels
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public List<int>? ImageIds { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<Color>? Colors { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<int>? MaterialIds { get; set; }
        public List<Material>? Materials { get; set; }
        public List<ProductImageVM>? productImageVMs { get; set; }

        public class ProductImageVM
        {
            public int Id { get; set; }
            public string ImgUrl { get; set; }
        }
    }
}
