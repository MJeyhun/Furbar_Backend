using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.SliderViewModels
{
    public class SliderCreateVM
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "The length of description must be 5 or more than 5!")]
        public string? Description { get; set; }
    }
}
