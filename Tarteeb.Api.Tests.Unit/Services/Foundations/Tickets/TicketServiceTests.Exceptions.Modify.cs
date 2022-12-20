//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}
