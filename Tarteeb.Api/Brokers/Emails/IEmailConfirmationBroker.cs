//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

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
