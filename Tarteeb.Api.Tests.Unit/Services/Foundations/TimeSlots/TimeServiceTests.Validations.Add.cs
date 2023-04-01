//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            //given
            Time noTime = null;
            var nullTimeException = new NullTimeException();

            var expectedTimeValidationException =
                new TimeValidationException(nullTimeException);

            //when
            ValueTask<Time> addTimeTask =
                this.timeService.AddTimeAsync(noTime);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(addTimeTask.AsTask);

            //then
            actualTimeValidationException.Should().BeEquivalentTo(
                expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTimeAsync(It.IsAny<Time>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTimeIsInvalidAndLogItAsync()
        {
            // given
            var invalidTime = new Time
            {
                HoursWorked = 0
            };

            var invalidTimeException = new InvalidTimeException();

            invalidTimeException.AddData(
                key: nameof(Time.Id),
                values: "Id is required");

            invalidTimeException.AddData(
                key: nameof(Time.HoursWorked),
                values: "Value is required");

            invalidTimeException.AddData(
               key: nameof(Time.CreatedDate),
               values: "Date is required");

            invalidTimeException.AddData(
               key: nameof(Time.UpdatedDate),
               values: "Date is required");

            var expectedTimeValidationException =
                new TimeValidationException(invalidTimeException);

            // when
            ValueTask<Time> addTimeTask = this.timeService.AddTimeAsync(invalidTime);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(addTimeTask.AsTask);

            // then
            actualTimeValidationException.Should().BeEquivalentTo(expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTimeAsync(It.IsAny<Time>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
