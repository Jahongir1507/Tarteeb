﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Tickets.Exceptions
{
    public class TicketDependencyValidationException : Xeption
    {
        public TicketDependencyValidationException(Xeption innerException)
            : base(message: "Ticket dependency validation error occurred, fix the errors and try again.", innerException)
        { }
    }
}
