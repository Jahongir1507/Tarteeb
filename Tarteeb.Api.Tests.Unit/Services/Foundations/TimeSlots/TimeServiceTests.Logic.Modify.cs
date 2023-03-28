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
        public async Task ShouldModifyTimeAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Time randomTime = CreateRandomModifyTime(randomDate);
            Time inputTime = randomTime;
            Time storageTime = inputTime.DeepClone();
            storageTime.UpdatedDate = randomTime.CreatedDate;
            Time updatedTime = inputTime;
            Time expectedTime = updatedTime.DeepClone();
            Guid timeId = inputTime.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(timeId)).ReturnsAsync(storageTime);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTimeAsync(inputTime)).ReturnsAsync(updatedTime);

            // when
            Time actualTime = await this.timeService.ModifyTimeAsync(inputTime);

            // then
            actualTime.Should().BeEquivalentTo(expectedTime);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(timeId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTimeAsync(inputTime), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
