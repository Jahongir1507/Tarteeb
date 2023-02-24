using System.Threading.Tasks;
using Tarteeb.Api.Models.EmailConfigurations;

namespace Tarteeb.Api.Brokers.EmailSenders
{
    public interface IEmailSenderBroker
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
