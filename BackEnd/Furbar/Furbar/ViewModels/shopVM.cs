using Furbar.Models;
using Furbar.ViewModels.Pagination;

namespace Furbar.ViewModels
{
    public class shopVM
    {
        public PaginationVM<Product> paginationVM { get; set; }
        public List<Product>? Products { get; set; }
    }
}
