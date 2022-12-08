//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Ticket> InsertTicketAsync(Ticket ticket);
        IQueryable<Ticket> SelectAllTickets();
        ValueTask<Ticket> SelectTicketByIdAsync(Guid id);
        ValueTask<Ticket> UpdateTicketAsync(Ticket ticket);
        ValueTask<Ticket> DeleteTicketAsync(Ticket ticket);
    }
}
