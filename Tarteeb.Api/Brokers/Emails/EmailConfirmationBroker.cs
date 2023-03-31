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
                this.Configuration.GetValue<string>("Postmark:ApiKey");

            string senderSignature = 
                this.Configuration.GetValue<string>("Postmark:SenderSignature");

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
