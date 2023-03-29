//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTimeIsNullAndLogItAsync()
        {
            // given
            Time nullTime = null;
            var nullTimeException = new NullTimeException();

            var expectedTimeValidationException =
                new TimeValidationException(nullTimeException);

            // whe
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(nullTime);

            var actualTimeValidationException =
                Assert.ThrowsAsync<TimeValidationException>(modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTimeAsync(It.IsAny<Time>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
