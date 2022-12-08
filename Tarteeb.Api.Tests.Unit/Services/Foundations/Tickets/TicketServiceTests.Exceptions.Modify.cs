//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Ticket someTicket = CreateRandomTicket();
            SqlException sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(failedTicketStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            //when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(someTicket);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    modifyTicketTask.AsTask);

            //then
            actualTicketDependencyException.Should()
                .BeEquivalentTo(expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(It.IsAny<Ticket>()), 
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(someTicket.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Ticket someTicket =
                CreateRandomTicket();

            Ticket foreignKeyConflictedTicket =
                someTicket;

            string randomString =
                GetRandomString();

            string exceptionMessage =
                randomString;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTicketReferenceException =
                new InvalidTicketReferenceException(foreignKeyConstraintConflictException);

            var ticketDependencyValidationException =
                new TicketDependencyValidationException(invalidTicketReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Ticket> modifyTicketTask =
                this.ticketService.ModifyTicketAsync(foreignKeyConflictedTicket);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                await Assert.ThrowsAsync<TicketDependencyValidationException>(
                    modifyTicketTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should()
                .BeEquivalentTo(ticketDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(foreignKeyConflictedTicket.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(ticketDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(foreignKeyConflictedTicket),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
