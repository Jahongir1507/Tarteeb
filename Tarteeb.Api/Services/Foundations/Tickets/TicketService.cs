//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public TicketService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        { 
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
           await this.storageBroker.InsertTicketAsync(ticket);
    }
}
