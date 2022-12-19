//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Extensions.Hosting;
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
                "Value is required",
                "Date is not recent",
                $"Date is same as {nameof(Ticket.CreatedDate)}");

            invalidTicketException.AddData(
                key: nameof(Ticket.CreatedUserId),
                values: "Id is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.UpdatedUserId),
                values: "Id is required");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(GetRandomDateTime);

            //when
            ValueTask<Ticket> addTicketTask = this.ticketService.ModifyTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(addTicketTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket invalidTicket = randomTicket;
            var invalidTicketException = new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.UpdatedDate),
                values: $"Date is same as {nameof(Ticket.CreatedDate)}");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Ticket> modifyTicketTask = this.ticketService.ModifyTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(modifyTicketTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(invalidTicket.Id), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket inputTicket = randomTicket;
            inputTicket.UpdatedDate = randomDateTime.AddSeconds(invalidSeconds);
            var invalidTicketException = new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.UpdatedDate),
                values: "Date is not recent");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Ticket> modifyTicketTask = this.ticketService.ModifyTicketAsync(inputTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(modifyTicketTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTicketDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegativeMInutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(dateTime);
            Ticket nonExistTicket = randomTicket;
            nonExistTicket.CreatedDate = dateTime.AddMinutes(randomNegativeMInutes);
            Ticket nullTicket = null;

            var notFoundTicketException = 
                new NotFoundTicketException(nonExistTicket.Id);

            var expectedTicketValidationException = new 
                TicketValidationException(notFoundTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(nonExistTicket.Id)).ReturnsAsync(nullTicket);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            //when
            ValueTask<Ticket> modifyTicketTask = 
                this.ticketService.ModifyTicketAsync(nonExistTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(modifyTicketTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(nonExistTicket.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogAsync()
        {
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomModifyTicket(randomDateTime);
            Ticket invalidTicket = randomTicket.DeepClone();
            Ticket storageTicket = invalidTicket.DeepClone();
            storageTicket.CreatedDate = storageTicket.CreatedDate.AddMinutes(randomMinutes);
            storageTicket.UpdatedDate = storageTicket.UpdatedDate.AddMinutes(randomMinutes);
            var invalidTicketException = new InvalidTicketException();
            Guid ticketId = invalidTicket.Id;

            invalidTicketException.AddData(
                key: nameof(Ticket.CreatedDate),
                values: $"Date is not the same as {nameof(Ticket.CreatedDate)}");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(ticketId)).ReturnsAsync(storageTicket);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    modifyTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(invalidTicket.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTicketValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
