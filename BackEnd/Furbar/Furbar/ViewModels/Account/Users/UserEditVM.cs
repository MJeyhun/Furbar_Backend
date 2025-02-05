namespace Furbar.ViewModels.Account.Users
{
    public class UserEditVM
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }

        public string? Username { get; set; }
        public bool? IsActive { get; set; } 
        public bool? IsSubscribed { get; set; } 

    }
}
