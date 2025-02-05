using Microsoft.AspNetCore.Identity;
using Furbar.Models.Accounts;

namespace Furbar.ViewModels.Account.Users
{
    public class ChangeRoleVM
    {
        public AppUser? User { get; set; }
        public IList<string>? UserRoles { get; set;}
        public List<IdentityRole>? AllRoles { get; set;}
    }
}
