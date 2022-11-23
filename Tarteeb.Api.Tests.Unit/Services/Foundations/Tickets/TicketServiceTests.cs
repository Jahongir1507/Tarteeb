//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Services.Foundations.Tickets;
using Tynamix.ObjectFiller;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITicketService ticketService;

        public TicketServiceTests()
        {
            this.storageBrokerMock= new Mock<IStorageBroker>();
            this.loggingBrokerMock= new Mock<ILoggingBroker>();

            this.ticketService = new TicketService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<Ticket> CreatedRandomTickets()
        {
            return CreateTicketFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Ticket CreateRandomTicket() =>
            CreateTicketFiller(dates: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
             new DateTimeRange(earliestDate: new DateTime()).GetValue();

        public static Filler<Ticket> CreateTicketFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Ticket>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}