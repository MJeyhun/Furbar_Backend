using Furbar.DAL;

using Furbar.Models.Accounts;
using Furbar.Services.Subscribtion;
using Microsoft.AspNetCore.Identity;

namespace Furbar
{
    public static class ServiceRegistration
    {
        public static void FurbarProjectRegistration(this IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddScoped<IMessageToSubscribe, MessageToSubscribe>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
  

        }
    }
}
