//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public interface ITicketService
    {
        ValueTask<Ticket> AddTicketAsync(Ticket ticket);
        IQueryable<Ticket> RetrieveAllTickets();
        ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId);
        ValueTask<Ticket> ModifyTicketAsync(Ticket ticket);
        ValueTask<Ticket> RemoveTicketByIdAsync(Guid ticketId);
    }
}