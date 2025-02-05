using Furbar.Models;
using Furbar.ViewModels.Pagination;

namespace Furbar.ViewModels
{
    public class blogVM
    {
        public PaginationVM<Blog>? paginationVM { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
