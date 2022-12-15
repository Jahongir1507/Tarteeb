//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedTicketServiceException = new FailedTicketServiceException(sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTickets()).Throws(sqlException);

            // when
            Action retrieveAllTicketAction = () =>
                this.ticketService.RetrieveAllTickets();

            TicketDependencyException actualTicketDependencyException =
                Assert.Throws<TicketDependencyException>(retrieveAllTicketAction);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(expectedTicketDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTickets(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTicketDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);
            var failedTicketServiceException = new FailedTicketServiceException(serviceException);

            var expectedTicketServiceException =
                new TicketServiceException(failedTicketServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTickets()).Throws(serviceException);

            // when
            Action retrieveAllTicketAction = () =>
                this.ticketService.RetrieveAllTickets();

            TicketServiceException actualTicketServiceException =
                Assert.Throws<TicketServiceException>(retrieveAllTicketAction);

            // then
            actualTicketServiceException.Should().BeEquivalentTo(expectedTicketServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTickets(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}