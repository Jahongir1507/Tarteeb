//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class EmailValidationException : Xeption
    {
        public EmailValidationException(Xeption innerException)
            : base(message: "Email validation error occurred, fix the errors and try again.", innerException)
        { }
    }
}
