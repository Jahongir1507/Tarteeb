//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Tickets.Exceptions
{
    public class TicketValidationException:Xeption
    {
        public TicketValidationException(Xeption innerException)
            :base(message:"Ticket validation error occured,fix the errors amd try again.",
                 innerException)
        { }
    }
}
