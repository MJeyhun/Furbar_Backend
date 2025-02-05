namespace Furbar.ViewModels.BlogViewModels
{
    public class BlogEditVM
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Username { get; set; }
        public IFormFile[]? Images { get; set; }
    }
}
