using Furbar.Models;
using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.ProductViewModels
{
    public class ProductCreateVM
    {
        public List<int> ColorIds { get; set; }
        public List<ProductColor>? ProductColors { get; set; }

      

        public List<int> CategoryIds { get; set; }
        public List<ProductCategory>? ProductCategories { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Description { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Range(typeof(double), "0.05", "99999999999")]
        public double Price { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public bool IsCompared { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int Count { get; set; }
   
        public string? Information { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Size { get; set; }
        public string? FrameSize { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Material { get; set; }
        public IFormFile[]? Images { get; set; }
    }
}
