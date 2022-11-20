//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IStorageBroker storageBroker;

        public TicketService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
            throw new System.NotImplementedException();
    }
}
