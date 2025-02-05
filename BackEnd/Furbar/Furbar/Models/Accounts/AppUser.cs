using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furbar.Models.Accounts
{
    public class AppUser:IdentityUser
    {
        public string? Fullname { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<Blog>? Blogs { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Product>? WihslistProducts { get; set; }
        public List<Product>? CartProducts { get; set; }
        public List<Testimonial>? Testimonials { get; set; }
        public string? ConnectionId { get; set; }
        public bool? IsSubscribed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
