﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidTicketId = Guid.Empty;

            var invalidTicketException =
                new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            // when
            ValueTask<Ticket> removeTicketByIdTask =
                this.ticketService.RemoveTicketByIdAsync(invalidTicketId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(removeTicketByIdTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfTicketIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomTicketId = Guid.NewGuid();
            Guid inputTicketId = randomTicketId;
            Ticket noTicket = null;

            var notFoundTicketException =
                new NotFoundTicketException(inputTicketId);

            var expectedTicketValidationException =
                new TicketValidationException(notFoundTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noTicket);

            // when
            ValueTask<Ticket> removeTicketByIdTask =
                this.ticketService.RemoveTicketByIdAsync(inputTicketId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    removeTicketByIdTask.AsTask);

            // then
            actualTicketValidationException.Should()
                .BeEquivalentTo(expectedTicketValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
              broker.DeleteTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}