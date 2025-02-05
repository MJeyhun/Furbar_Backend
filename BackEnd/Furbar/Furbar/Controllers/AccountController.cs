using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MimeKit.Text;
using System.Runtime.CompilerServices;
using Furbar.Models.Accounts;
using Furbar.ViewModels.Account;
using Furbar.Helpers.Enums;

namespace Furbar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = new();
            appUser.Email = registerVM.Email;
            appUser.UserName = registerVM.Username;
            appUser.Fullname = registerVM.Fullname;
            appUser.IsActive = true;
            appUser.IsSubscribed = false;
            appUser.CreatedDate= DateTime.UtcNow;
            appUser.IsDeleted= false;

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = appUser.Id, token }
            , Request.Scheme
            , Request.Host.ToString());


            //Roles
            await _userManager.AddToRoleAsync(appUser, RoleEnums.Admin.ToString());


            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("purposeistry@gmail.com"));
            email.To.Add(MailboxAddress.Parse(appUser.Email));
            email.Subject = "Verify email";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/Verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", appUser.Fullname);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("purposeistry@gmail.com", "meoajiatmyxftoik");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(Login));

        }


        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM) 
        {
            if (!ModelState.IsValid) return View();
            AppUser exsistUser = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
            if (exsistUser == null)
            {
                ModelState.AddModelError("Email", "User not Found");
                return View();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

            string link = Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token },
                          Request.Scheme, Request.Host.ToString());
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("purposeistry@gmail.com"));
            email.To.Add(MailboxAddress.Parse(exsistUser.Email));
            email.Subject = "Verify password reset email";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/Verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", exsistUser.Fullname);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("purposeistry@gmail.com", "meoajiatmyxftoik");
            smtp.Send(email);
            smtp.Disconnect(true);


            return RedirectToAction(nameof(VerifyEmail));
        }

        public IActionResult ResetPassword(string userId,string token) 
        {
            ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
            {
                UserId = userId,
                Token = token
            };
            return View(resetPasswordVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser exsistUser = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            bool isUsed =await _userManager.VerifyUserTokenAsync(exsistUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPasswordVM.Token);
            if (!isUsed)
            {
                return Content("The same link cannot be used again");
            }; 

            if (exsistUser == null) return NotFound();
            if (await _userManager.CheckPasswordAsync(exsistUser, resetPasswordVM.Password))
            {
                ModelState.AddModelError("", "This Password is your old password");
                return View(resetPasswordVM);
            }

            await _userManager.ResetPasswordAsync(exsistUser, resetPasswordVM.Token, resetPasswordVM.Password);
            await _userManager.UpdateSecurityStampAsync(exsistUser);

            return RedirectToAction(nameof(Login));
        }






        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string ? ReturnUrl)
        {
            
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username(Email) or Password invalid!");
                    return View(loginVM);
                }
            }
            if (!user.IsActive) 
            {
                ModelState.AddModelError("", "Hesabınız bloklanıb");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabınız bloklanıb");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username(Email) or Password invalid!");
                return View(loginVM);
            }

            //sign in

            if (await _userManager.IsInRoleAsync(user, RoleEnums.Admin.ToString()))
            {
                return RedirectToAction("index","dashboard", new { Area = "AdminArea" });
            }

            if (ReturnUrl != null)
            {
                return RedirectToAction(ReturnUrl);
            }

            await _signInManager.SignInAsync(user, false);


            return RedirectToAction("index", "home");
        }


        public async Task<IActionResult> Logout() 
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
