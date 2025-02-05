using Furbar.Models.Accounts;

namespace Furbar.ViewModels.Account.Users
{
    public class UserDetailVM
    {
        public AppUser? User { get; set; }
        public IList<String>? UserRoles { get; set; }
    }
}
