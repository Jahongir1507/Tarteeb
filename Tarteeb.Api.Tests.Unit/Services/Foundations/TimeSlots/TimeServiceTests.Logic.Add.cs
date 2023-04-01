//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Foundations.Times;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldAddTimeAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time inputTime = randomTime;
            Time storagedTime = inputTime;
            Time expectedTime = storagedTime.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTimeAsync(inputTime)).ReturnsAsync(storagedTime);

            //when
            Time actualTime = await this.timeService.AddTimeAsync(inputTime);

            //then
            actualTime.Should().BeEquivalentTo(expectedTime);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTimeAsync(inputTime), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
