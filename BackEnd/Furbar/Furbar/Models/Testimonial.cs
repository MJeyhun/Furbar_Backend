using Furbar.Models.Accounts;

namespace Furbar.Models
{
    public class Testimonial:BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Content { get; set; }
    }
}
