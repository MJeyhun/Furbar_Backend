using Furbar.Models;

namespace Furbar.ViewModels.ProductViewModels
{
    public class ProductDetailVM
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsCompared { get; set; }
        public int Count { get; set; }
        public string? Information { get; set; }
        public string? Size { get; set; }
        public string? FrameSize { get; set; }
        public string? Material { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductCategory>? ProductCategories { get; set; }
    }
}
