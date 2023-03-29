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
        public async Task ShouldRemoveTeamByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTimeId = randomId;
            Time randomTime = CreateRandomTime();
            Time storageTime = randomTime;
            Time expectedInputTime = storageTime;
            Time deletedTime = expectedInputTime;
            Time expectedTime = deletedTime.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(inputTimeId))
                    .ReturnsAsync(storageTime);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTimeAsync(expectedInputTime))
                    .ReturnsAsync(deletedTime);

            // when
            Time actualTime =
                await this.timeService.RemoveTimeByIdAsync(inputTimeId);

            // then
            actualTime.Should().BeEquivalentTo(expectedTime);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(inputTimeId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTimeAsync(expectedInputTime), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
