//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Ticket someTicket = CreateRandomTicket();
            SqlException sqlException = CreateSqlException();
            var failedTicketStorageException = new FailedTicketStorageException(sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                .Throws(sqlException);

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(someTicket);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(addTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Ticket someTicket = CreateRandomTicket();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsTicketException =
                new AlreadyExistsTicketException(duplicateKeyException);

            var expectedTicketDependencyValidationException =
                new TicketDependencyValidationException(alreadyExistsTicketException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(someTicket);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                await Assert.ThrowsAsync<TicketDependencyValidationException>(addTicketTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should().BeEquivalentTo(
                expectedTicketDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedTicketDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertTicketAsync(
                It.IsAny<Ticket>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Ticket someTicket = CreateRandomTicket();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTicketException = new LockedTicketException(dbUpdateConcurrencyException);
            var expectedTicketDependencyValidationException = new TicketDependencyValidationException(lockedTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Ticket> addTicketTask = this.ticketService.AddTicketAsync(someTicket);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                 await Assert.ThrowsAsync<TicketDependencyValidationException>(addTicketTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should().BeEquivalentTo(expectedTicketDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedTicketDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Ticket someTicket = CreateRandomTicket();
            var serviceException = new Exception();

            var failedTicketServiceException =
                new FailedTicketServiceException(serviceException);

            var expectedTicketServiceException =
                new TicketServiceException(failedTicketServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(serviceException);

            // when
            ValueTask<Ticket> addTicketTask =
                this.ticketService.AddTicketAsync(someTicket);

            TicketServiceException actualTicketServiceException =
                await Assert.ThrowsAsync<TicketServiceException>(addTicketTask.AsTask);

            // then
            actualTicketServiceException.Should().BeEquivalentTo(
                expectedTicketServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(It.IsAny<Ticket>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
