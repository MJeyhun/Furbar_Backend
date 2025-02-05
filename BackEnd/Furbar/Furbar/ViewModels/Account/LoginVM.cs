using System.ComponentModel.DataAnnotations;

namespace Furbar.ViewModels.Account
{
    public class LoginVM
    {
        [System.ComponentModel.DataAnnotations.Required, StringLength(100)]
        public string? UsernameOrEmail { get; set; }
        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
