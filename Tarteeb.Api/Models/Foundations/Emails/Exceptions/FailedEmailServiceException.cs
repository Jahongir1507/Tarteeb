//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class FailedEmailServiceException : Xeption
    {
        public FailedEmailServiceException(Exception innerException)
            : base(message: "Failed email service error occurred, contact support.", innerException)
        { }
    }
}
