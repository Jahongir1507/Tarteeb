//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class EmailDependencyException : Xeption
    {
        public EmailDependencyException(Xeption innerException)
            : base(message: "Email dependency error occurred, contact support.", innerException)
        { }
    }
}
