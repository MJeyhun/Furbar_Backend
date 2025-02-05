using Furbar.Models;
using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.ProductViewModels
{
    public class ProductEditVM
    {
        public List<int> ColorIds { get; set; }
        public List<int>? ExistColorIds { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public List<int>? ExistCategoryIds { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(typeof(double), "0.05", "99999999999")]
        public double Price { get; set; }
        public bool IsCompared { get; set; }
        public int Count { get; set; }
        public string? Information { get; set; }
        public string? Size { get; set; }
        public string? FrameSize { get; set; }
        public string? Material { get; set; }
        public IFormFile[]? Images { get; set; }
    }
}
