//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Ticket noTicket = null;
            var nullTicketException = new NullTicketException();

            var expectedTicketValidationException =
                new TicketValidationException(nullTicketException);

            // when
            ValueTask<Ticket> addTicketTask =
                this.ticketService.AddTicketAsync(noTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
