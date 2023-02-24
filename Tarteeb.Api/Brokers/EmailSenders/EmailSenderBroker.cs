//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Tarteeb.Api.Models.EmailConfigurations;

namespace Tarteeb.Api.Brokers.EmailSenders
{
    public class EmailSenderBroker : IEmailSenderBroker
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailSenderBroker(IConfiguration configuration) 
        {
            this.emailConfiguration = new EmailConfiguration();
            configuration.Bind("Email", this.emailConfiguration);
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(this.emailConfiguration.EmailAddress));
            email.To.Add(MailboxAddress.Parse(emailMessage.To));
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailMessage.Body.ToString() };

            var smtp = new SmtpClient();

            await smtp.ConnectAsync(this.emailConfiguration.Host, 587, 
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(this.emailConfiguration.EmailAddress, 
                this.emailConfiguration.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
