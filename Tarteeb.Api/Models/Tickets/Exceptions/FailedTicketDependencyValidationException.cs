//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Tickets.Exceptions
{
    public class FailedTicketDependencyValidationException : Xeption
    {
        public FailedTicketDependencyValidationException(Exception innerException)
            : base(message: "Failed ticket dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
