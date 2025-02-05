namespace Furbar.ViewModels.BlogViewModels
{
    public class BlogCreateVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string? Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Content { get; set; }  
        
        [System.ComponentModel.DataAnnotations.Required]
        public string? Username { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public IFormFile[]? Images { get; set; }
    }
}
