//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Tickets.Exceptions
{
    public class LockedTicketException : Xeption
    {
        public LockedTicketException(Exception innerException)
            : base(message: "Ticket is locked, please try again.", innerException)
        { }
    }
}
