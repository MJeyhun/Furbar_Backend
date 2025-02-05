using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace   Furbar.ViewModels.Account
{
    public class RegisterVM
    {
        [System.ComponentModel.DataAnnotations.Required, StringLength(100)]
        public string? Fullname { get; set; }
        [System.ComponentModel.DataAnnotations.Required, StringLength(100)]
        public string? Username { get; set; }
        [System.ComponentModel.DataAnnotations.Required,DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password)]
        public string? Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string? RePassword { get; set; }
    }
}
