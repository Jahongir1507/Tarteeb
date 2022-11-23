//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public class TicketService: ITicketService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public TicketService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public TicketService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public IQueryable<Ticket> RetriveAllTickets()
        {
            throw new System.NotImplementedException();
        }       
    }
}