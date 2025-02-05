using System.ComponentModel.DataAnnotations;

namespace   Furbar.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required]
        [DataType(DataType.Password), MinLength(6)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password), Compare(nameof(Password)),MinLength(6)]
        public string? ConfirmPassword { get; set; }

        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}
