//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exception;
using Xunit;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTicketIsNullAndLogItAsync()
        {
            //given
            Ticket nullTicket = null;
            NullTicketException nullTicketException = new NullTicketException();

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
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
