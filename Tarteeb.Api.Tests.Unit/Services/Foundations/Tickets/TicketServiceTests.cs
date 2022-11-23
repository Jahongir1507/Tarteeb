//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//==================================================


using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Services.Foundations.Tickets;
using Tynamix.ObjectFiller;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly ITicketService ticketService;

        public TicketServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.ticketService = new TicketService(
                storageBroker: this.storageBrokerMock.Object);
        }

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();

            return new TheoryData<int>
            {
                randomNumber
            };
        }

        private static IQueryable<Ticket> CreateRandomTickets()
        {
            return CreateTicketFiller()
                .Create(count: GetRandomNumber())
                .AsQueryable();
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Filler<Ticket> CreateTicketFiller()
        {
            var filler = new Filler<Ticket>();
            DateTimeOffset dates = GetRandomDateTime();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
