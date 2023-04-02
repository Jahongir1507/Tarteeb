//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class EmailDependencyValidationException : Xeption
    {
        public EmailDependencyValidationException(Xeption innerException)
            : base(message: "Email dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
