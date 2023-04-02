//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Models.Foundations.Emails.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Emails
{
    public partial class EmailService
    {
        private static void ValidateEmailNotNull(Email email)
        {
            if (email is null)
            {
                throw new NullEmailException();
            }
        }
    }
}
