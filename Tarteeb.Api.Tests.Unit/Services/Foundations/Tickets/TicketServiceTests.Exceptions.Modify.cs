//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Moq;
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
                broker.SelectTicketByIdAsync(ticketId))
                    .ThrowsAsync(databaseUpdateException);

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
    }
}
