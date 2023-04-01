//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidTimeId = Guid.Empty;
            var invalidTimeException = new InvalidTimeException();

            invalidTimeException.AddData(
                key: nameof(Time.Id),
                values: "Id is required");

            var expectedTimeValidationException =
                new TimeValidationException(invalidTimeException);

            // when
            ValueTask<Time> removeTimeByIdTask =
                this.timeService.RemoveTimeByIdAsync(invalidTimeId);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(
                    removeTimeByIdTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTimeAsync(It.IsAny<Time>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfTeamIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTimeId = randomId;
            Time noTime = null;

            var notFoundTimeException =
                new NotFoundTimeException(inputTimeId);

            var expectedTimeValidationException =
                new TimeValidationException(notFoundTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noTime);

            // when
            ValueTask<Time> removeTimeByIdTask =
                this.timeService.RemoveTimeByIdAsync(inputTimeId);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(
                    removeTimeByIdTask.AsTask);

            // then
            actualTimeValidationException.Should().BeEquivalentTo(expectedTimeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(inputTimeId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTimeAsync(It.IsAny<Time>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
