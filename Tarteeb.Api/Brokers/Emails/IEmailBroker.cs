//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using PostmarkDotNet;
using Tarteeb.Api.Models.Foundations.Emails;

namespace Tarteeb.Api.Brokers.Emails
{
    public interface IEmailBroker
    {
        Task<PostmarkResponse> SendEmailAsync(Email email);
    }
}
