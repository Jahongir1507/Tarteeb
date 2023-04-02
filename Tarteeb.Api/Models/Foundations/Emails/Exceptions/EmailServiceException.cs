//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class EmailServiceException : Xeption
    {
        public EmailServiceException(Xeption innerException)
            : base(message: "Email service error occurred, contact support.", innerException)
        { }
    }
}
