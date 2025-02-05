using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MimeKit.Text;
using System.Runtime.CompilerServices;
using Furbar.Helpers.Enums;
using Furbar.Models.Accounts;
using Furbar.ViewModels.Account;
using Furbar.Services.Subscribtion;

namespace Furbar.Services.Subscribtion
{
    public class MessageToSubscribe : IMessageToSubscribe
    {
        public void SendMessageSubscribed(List<AppUser> users,string objectName)
        {
            var email = new MimeMessage();
            List<AppUser> subsribedUsers = new();
            foreach (var user in users) 
            {
                if (user.IsSubscribed==true) 
                {
                    subsribedUsers.Add(user);
                }
            }
            email.From.Add(MailboxAddress.Parse("purposeistry@gmail.com"));
            foreach(var user in subsribedUsers)
            {
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = $" New {objectName} added";
                string body = string.Empty;

                using (StreamReader reader = new StreamReader("wwwroot/Template/Subscribe.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{{Fullname}}", user.Fullname);
                body = body.Replace("{{objectName}}", objectName);

                email.Body = new TextPart(TextFormat.Html) { Text = body };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("purposeistry@gmail.com", "meoajiatmyxftoik");
                smtp.Send(email);
                smtp.Disconnect(true);

            }

        }
    }
}
