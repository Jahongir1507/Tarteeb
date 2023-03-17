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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdAsyncIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            // when
            ValueTask<Ticket> retrieveTicketByIdTask =
                this.ticketService.RetrieveTicketByIdAsync(someId);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(retrieveTicketByIdTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(expectedTicketDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTicketServiceException =
                new FailedTicketServiceException(serviceException);

            var expectedTicketServiceExcpetion =
                new TicketServiceException(failedTicketServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            // when
            ValueTask<Ticket> retrieveTicketById =
                this.ticketService.RetrieveTicketByIdAsync(someId);

            TicketServiceException actualTeamServiceException =
                await Assert.ThrowsAsync<TicketServiceException>(retrieveTicketById.AsTask);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(expectedTicketServiceExcpetion);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTicketServiceExcpetion))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}