using Microsoft.AspNetCore.Identity;

namespace Furbar.Helpers
{
    public class CustomIdentityErrorDescriber: IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        { return new IdentityError { Code = nameof(DuplicateUserName), 
            Description = $"'{userName}' adlı istifadəçi artıq mövcuddur!" };
        }
    }
}
