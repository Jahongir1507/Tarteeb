//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(someDateTime);
            Ticket someTicket = randomTicket;
            Guid ticketId = someTicket.Id;
            SqlException sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(someTicket);

            TicketDependencyException actualTicketDependencyException =
              await Assert.ThrowsAsync<TicketDependencyException>(
                  modifyTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(ticketId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(someTicket), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket SomeTicket = randomTicket;
            Guid ticketId = SomeTicket.Id;
            SomeTicket.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedTicketException =
                new FailedTicketStorageException(databaseUpdateException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(ticketId)).ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(SomeTicket);

            TicketDependencyException actualTicketDependencyException =
              await Assert.ThrowsAsync<TicketDependencyException>(
                  modifyTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(ticketId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket someTicket = randomTicket;
            someTicket.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid ticketId = someTicket.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTicketException =
                new LockedTicketException(databaseUpdateConcurrencyException);

            var expectedTicketDependencyValidationException =
                new TicketDependencyValidationException(lockedTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(ticketId)).ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(someTicket);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                await Assert.ThrowsAsync<TicketDependencyValidationException>(modifyTicketTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should().BeEquivalentTo(
                expectedTicketDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(ticketId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket someTicket = randomTicket;
            someTicket.CreatedDate = randomDateTime.AddMinutes(minuteInPast);
            var serviceException = new Exception();

            var failedTicketException =
                new FailedTicketServiceException(serviceException);

            var expectedTicketServiceException =
                new TicketServiceException(failedTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(someTicket.Id)).ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(someTicket);

            TicketServiceException actualTicketServiceException =
                await Assert.ThrowsAsync<TicketServiceException>(
                    modifyTicketTask.AsTask);

            // then
            actualTicketServiceException.Should().BeEquivalentTo(
                expectedTicketServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(someTicket.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
