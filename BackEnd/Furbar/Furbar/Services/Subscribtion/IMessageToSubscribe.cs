using Furbar.Models.Accounts;

namespace Furbar.Services.Subscribtion
{
    public interface IMessageToSubscribe
    {
        void SendMessageSubscribed(List<AppUser> users, string objectName); 
    }
}
