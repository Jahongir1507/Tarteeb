//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given 
            var invalidTicketId = Guid.Empty;

            var invalidTicketException =
                new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            var expectedTicketValidationException = new
                TicketValidationException(invalidTicketException);

            // when 
            ValueTask<Ticket> retrieveTicketByIdTask =
                this.ticketService.RetrieveTicketByIdAsync(invalidTicketId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(retrieveTicketByIdTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfTicketNotFoundAndLogItAsync()
        {
            // given
            Guid someTicketId = Guid.NewGuid();
            Ticket noTicket = null;

            var notFoundTicketValidationException =
                new NotFoundTicketException(someTicketId);

            var expectedValidationException =
                new TicketValidationException(notFoundTicketValidationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noTicket);

            // when
            ValueTask<Ticket> retrieveByIdTicketTask =
                this.ticketService.RetrieveTicketByIdAsync(someTicketId);

            TicketValidationException actualValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    retrieveByIdTicketTask.AsTask);

            // then
            actualValidationException.Should().BeEquivalentTo(expectedValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}