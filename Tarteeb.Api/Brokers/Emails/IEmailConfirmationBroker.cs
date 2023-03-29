using System.Threading.Tasks;
using PostmarkDotNet;

namespace Tarteeb.Api.Brokers.Emails
{
    public interface IEmailConfirmationBroker
    {
        Task<PostmarkResponse> SendingConfirmationMessage(
            string userEmail, string messageHtmlBody);
    }
}
