//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Emails.Exceptions
{
    public class InvalidEmailException : Xeption
    {
        public InvalidEmailException()
           : base(message: "Email is invalid.")
        { }

        public InvalidEmailException(Exception innerException)
            : base(message: "Email is invalid. See inner exception for more details.", innerException)
        { }
    }
}
