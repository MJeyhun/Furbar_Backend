using Furbar.Models.Accounts;

namespace Furbar.Models
{
    public class Review:BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string? Message { get; set; }

        public int StarReview { get; set; } = 0;
    }
}
