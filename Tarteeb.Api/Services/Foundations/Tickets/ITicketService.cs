//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================
 
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public interface ITicketService
    {
        ValueTask<Ticket> AddTicketAsync(Ticket ticket);
        ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId);
        ValueTask<Ticket> ModifyTicketAsync(Ticket ticket);
    }
}