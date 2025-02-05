using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.TestimonialViewModels
{
    public class TestimonialCreateVM
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
