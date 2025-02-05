using Furbar.Models;

namespace Furbar.ViewModels
{
    public class blogDetailVM
    {
        public Blog? Blog { get; set; }
        public int CommentCount{ get; set; }

        public List<Blog>? Blogs { get; set; }
    }
}
