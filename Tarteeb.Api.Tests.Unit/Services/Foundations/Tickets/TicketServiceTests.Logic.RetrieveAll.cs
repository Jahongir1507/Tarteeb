//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTickets()
        {
            // given
            IQueryable<Ticket> randomTickets = CreatedRandomTickets();
            IQueryable<Ticket> storageTickets = randomTickets;
            IQueryable<Ticket> expectedTickets = storageTickets;

            this.storageBrokerMock.Setup(broker =>
            broker.SelectAllTickets())
                .Returns(storageTickets);

            // when 
            IQueryable<Ticket> actualTickets =
                this.ticketService.RetrieveAllTickets();

            // then
            actualTickets.Should().BeEquivalentTo(expectedTickets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTickets(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}