using Furbar.Models;
using Furbar.Models.Accounts;

namespace Furbar.ViewModels.BlogViewModels
{
    public class BlogDetailVM
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public AppUser? AppUser { get; set; }
        public List<BlogImage>? BlogImages { get; set; }
    }
}
