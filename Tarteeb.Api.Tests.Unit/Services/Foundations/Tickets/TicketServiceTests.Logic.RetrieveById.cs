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
        public async Task ShouldRetriveTicketByIdAsync()
        {
            //given
            Guid randomTicketId = Guid.NewGuid();
            Guid inputTicketId = randomTicketId;
            Ticket randomTicket = CreateRandomTicket();
            Ticket storedTicket = randomTicket;
            Ticket expectedTicket = storedTicket.DeepClone();

            this.storageBrokerMock.Setup(broker =>
              broker.SelectTicketByIdAsync(randomTicketId)).ReturnsAsync(storedTicket);

            //when
            Ticket actualTicket =
                await this.ticketService.RetrieveTicketByIdAsync(inputTicketId);

            //then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(inputTicketId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}