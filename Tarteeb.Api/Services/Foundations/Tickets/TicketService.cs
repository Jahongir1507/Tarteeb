//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService : ITicketService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TicketService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicket(ticket);

            return await this.storageBroker.InsertTicketAsync(ticket);
        });

        public ValueTask<Ticket> ModifyTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicketIsNotNull(ticket);
            var maybeTicket = await this.storageBroker.SelectTicketByIdAsync(ticket.Id);

            return await this.storageBroker.UpdateTicketAsync(ticket);
        });
    }
}
