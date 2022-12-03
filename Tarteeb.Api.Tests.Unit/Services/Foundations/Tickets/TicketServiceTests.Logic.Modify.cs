//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldModifyTicketAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket();
            Ticket inputTicket = randomTicket;
            inputTicket.UpdatedDate = randomDate.AddMinutes(1);
            Ticket storageTicket = inputTicket;
            Ticket updatedTicket = inputTicket;
            Ticket expectedTicket = updatedTicket.DeepClone();
            Guid inputTicketId = inputTicket.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(inputTicketId))
                    .ReturnsAsync(storageTicket);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTicketAsync(inputTicket))
                    .ReturnsAsync(updatedTicket);

            //when
            Ticket actualTicket =
                await this.ticketService.
                    ModifyTicketAsync(inputTicket);

            //then
            actualTicket.Should().
                BeEquivalentTo(expectedTicket);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(inputTicketId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(inputTicket), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
