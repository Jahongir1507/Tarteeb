//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
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
            ValueTask<Time> removeTimeById =
                this.timeService.RemoveTimeByIdAsync(invalidTimeId);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(
                    removeTimeById.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTimeAsync(It.IsAny<Time>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
