//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Tickets.Exceptions
{
    public class NotFoundTicketException : Xeption
    {
        public NotFoundTicketException(Guid ticketId)
            : base(message: $"Couldn't find ticket with id: {ticketId}.")
        { }
    }
}
