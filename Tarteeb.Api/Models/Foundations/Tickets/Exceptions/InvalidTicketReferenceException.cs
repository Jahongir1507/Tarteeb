//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Tickets.Exceptions
{
    public class InvalidTicketReferenceException : Xeption
    {
        public InvalidTicketReferenceException(Exception innerException)
            : base(message: "Invalid ticket reference error occurred.", innerException)
        { }
    }
}
