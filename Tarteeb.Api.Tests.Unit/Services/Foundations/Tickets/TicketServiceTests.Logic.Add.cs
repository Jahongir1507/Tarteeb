//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldAddTicketAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket inputTicket = randomTicket;
            Ticket persistedTicket = inputTicket;
            Ticket expectedTicket = persistedTicket.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTicketAsync(inputTicket))
                    .ReturnsAsync(persistedTicket);

            // when
            Ticket actualTicket = await this.ticketService.AddTicketAsync(inputTicket);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(inputTicket), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
