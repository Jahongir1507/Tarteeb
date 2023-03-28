using System;
using System.Linq;
using FluentAssertions;
using Tarteeb.Api.Models.Foundations.Times;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Times
{
    public partial class TimeServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTimes()
        {
            //given
            IQueryable<Time> randomTimes = CreateRandomTimes();
            IQueryable<Time> storageTimes = randomTimes;
            IQueryable<Time> expectedTimes = storageTimes;

            this.storageBrokerMock.Setup(broker=>
                broker.SelectAllTimes()).Returns(storageTimes);
            //when
            IQueryable<Time> actualTime =
               this.timeService.RetrieveAllTimes();

            //then
            actualTime.Should().BeEquivalentTo(expectedTimes);

            this.storageBrokerMock.Verify(broker=> 
                broker.SelectAllTimes(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
