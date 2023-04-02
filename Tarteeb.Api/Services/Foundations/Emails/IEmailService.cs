//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Emails;

namespace Tarteeb.Api.Services.Foundations.Emails
{
    public interface IEmailService
    {
        ValueTask<Email> SendEmailAsync(Email email);
    }
}