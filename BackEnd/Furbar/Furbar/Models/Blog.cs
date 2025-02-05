using Furbar.Models.Accounts;

namespace Furbar.Models
{
    public class Blog:BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<BlogImage>? BlogImages { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
