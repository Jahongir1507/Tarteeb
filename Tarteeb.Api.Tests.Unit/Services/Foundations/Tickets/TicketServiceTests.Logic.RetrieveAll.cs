//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//==================================================


using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTickets()
        {
            //given
            IQueryable<Ticket> randomTickets = CreateRandomTickets();
            IQueryable<Ticket> storageTickets = randomTickets;
            IQueryable<Ticket> expextedTickets = storageTickets;

            this.storageBrokerMock.Setup(broker =>
            broker.SelectAllTickets())
                .Returns(storageTickets);

            //when
            IQueryable<Ticket> actualTicket = this.ticketService.RetriveAllTickets();

            //then
            actualTicket.Should()
                        .BeEquivalentTo(expextedTickets);

            this.storageBrokerMock.Verify(broker => broker.SelectAllTickets(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();            
        }
    }
}
