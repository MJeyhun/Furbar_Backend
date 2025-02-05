using Furbar.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Furbar.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        public List<Blog>? Blogs { get; set; }
    }
}
