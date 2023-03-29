//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someTimeId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTimeException =
                new LockedTimeException(databaseUpdateConcurrencyException);

            var expectedTimeDependencyValidationException =
                new TimeDependencyValidationException(lockedTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Time> removeTimeByIdTask =
                this.timeService.RemoveTimeByIdAsync(someTimeId);

            TimeDependencyValidationException actualTimeDependencyValidationException =
                await Assert.ThrowsAsync<TimeDependencyValidationException>(
                    removeTimeByIdTask.AsTask);

            //then
            actualTimeDependencyValidationException.Should()
                .BeEquivalentTo(expectedTimeDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTimeAsync(It.IsAny<Time>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
