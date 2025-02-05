using Furbar.Models.Accounts;

namespace Furbar.Models
{
    public class Comment:BaseEntity
    {
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Message { get; set; }
    }
}
