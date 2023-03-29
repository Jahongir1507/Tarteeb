//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================


using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PostmarkDotNet;

namespace Tarteeb.Api.Brokers.Emails
{
    public class EmailConfirmationBroker : IEmailConfirmationBroker
    {
        private readonly IConfiguration Configuration;

        public EmailConfirmationBroker(IConfiguration configuration) =>
            this.Configuration = configuration;

        public async Task<PostmarkResponse> SendingConfirmationMessage(
            string userEmail, string messageHtmlBody)
        {
            string apiKey = 
                this.Configuration.GetSection("Postmark").GetValue<string>("ApiKey");

            string senderSignature = 
                this.Configuration.GetSection("Postmark").GetValue<string>("SenderSignature");

            var client = new PostmarkClient(apiKey);

            var message = new PostmarkMessage()
            {
                To = userEmail,
                From = senderSignature,
                TrackOpens = true,
                Subject = "Email Confirmation",
                HtmlBody = messageHtmlBody
            };

            var result = await client.SendMessageAsync(message);

            return result;
        }
    }
}
