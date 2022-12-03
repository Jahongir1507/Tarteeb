//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnAddIfTicketIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
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

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset anotherRandomDate = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket invalidTicket = randomTicket;
            invalidTicket.UpdatedDate = anotherRandomDate;
            var invalidTicketException = new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.CreatedDate),
                values: $"Date is not same as {nameof(Ticket.UpdatedDate)}");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(
                It.Is(SameExceptionAs(expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertTicketAsync(
                It.IsAny<Ticket>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset invalidRandomDateTime = randomDateTime.AddSeconds(invalidSeconds);
            Ticket randomInvalidTicket = CreateRandomTicket(invalidRandomDateTime);
            Ticket invalidTicket = randomInvalidTicket;
            var invalidTicketException = new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.CreatedDate),
                values: "Date is not recent");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
