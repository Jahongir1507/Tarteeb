//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTicketIsNullAndLogItAsync()
        {
            //given
            Ticket nullTicket = null;
            var nullTicketException = new NullTicketException();

            var expectedTicketValidationException =
                new TicketValidationException(nullTicketException);

            //when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(nullTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    modifyTicketTask.AsTask);

            //then
            actualTicketValidationException.Should()
                .BeEquivalentTo(expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTicketIsInvalidAndLogItAsync(
            string invalidString)
        {
            //given
            var invalidTicket = new Ticket
            {
                Title = invalidString
            };

            var invalidTicketException = new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Title),
                values: "Text is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Deadline),
                values: "Value is required");

            invalidTicketException.AddData(
               key: nameof(Ticket.CreatedDate),
               values: "Value is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.UpdatedDate),
                values: "Value is required");

            invalidTicketException.AddData(
               key: nameof(Ticket.CreatedUserId),
               values: "Id is required");

            invalidTicketException.AddData(
              key: nameof(Ticket.UpdatedUserId),
              values: "Id is required");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(GetRandomDateTime);

            //when
            ValueTask<Ticket> addTicketTask = this.ticketService.ModifyTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTicketValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
