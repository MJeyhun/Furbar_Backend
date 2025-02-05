using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.SliderViewModels
{
    public class SliderEditVM
    {
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
        [MinLength(5, ErrorMessage = "The length of description must be 5 or more than 5!")]
        public string? Description { get; set; }
    }
}
