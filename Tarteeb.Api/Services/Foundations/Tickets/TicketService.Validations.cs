//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exception;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService
    {
        private void ValidateTicketIsNotNull(Ticket ticket) 
        {
            if (ticket is null)
            {
                throw new NullTicketException();
            }
        }
    }
}
