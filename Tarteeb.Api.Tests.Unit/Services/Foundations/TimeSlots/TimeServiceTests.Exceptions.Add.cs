//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Time someTime = CreateRandomTime();
            SqlException sqlException = CreateSqlException();
            var failedTimeStorageException = new FailedTimeStorageException(sqlException);

            var expectedTimeDependencyException =
                new TimeDependencyException(failedTimeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Time> addTimeTask = this.timeService.AddTimeAsync(someTime);

            TimeDependencyException actualTimeDependencyException =
                await Assert.ThrowsAsync<TimeDependencyException>(addTimeTask.AsTask);

            // then
            actualTimeDependencyException.Should().BeEquivalentTo(expectedTimeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTimeDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTimeAsync(It.IsAny<Time>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Time someTime = CreateRandomTime();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsTimeException =
                new AlreadyExistsTimeException(duplicateKeyException);

            var expectedTimeDependencyValidationException =
                new TimeDependencyValidationException(alreadyExistsTimeException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Time> addTimeTask = this.timeService.AddTimeAsync(someTime);

            TimeDependencyValidationException actualTimeDependencyValidationException =
                await Assert.ThrowsAsync<TimeDependencyValidationException>(addTimeTask.AsTask);

            // then
            actualTimeDependencyValidationException.Should().BeEquivalentTo(
                expectedTimeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedTimeDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertTimeAsync(
                It.IsAny<Time>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Time someTime = CreateRandomTime();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTimeException = new LockedTimeException(dbUpdateConcurrencyException);
            var expectedTimeDependencyValidationException = new TimeDependencyValidationException(lockedTimeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Time> addTimeTask = this.timeService.AddTimeAsync(someTime);

            TimeDependencyValidationException actualTimeDependencyValidationException =
                 await Assert.ThrowsAsync<TimeDependencyValidationException>(addTimeTask.AsTask);

            // then
            actualTimeDependencyValidationException.Should().BeEquivalentTo(expectedTimeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedTimeDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertTimeAsync(It.IsAny<Time>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Time someTime = CreateRandomTime();
            var serviceException = new Exception();

            var failedTimeServiceException =
                new FailedTimeServiceException(serviceException);

            var expectedTimeServiceException =
                new TimeServiceException(failedTimeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(serviceException);

            // when
            ValueTask<Time> addTimeTask =
                this.timeService.AddTimeAsync(someTime);

            TimeServiceException actualTimeServiceException =
                await Assert.ThrowsAsync<TimeServiceException>(addTimeTask.AsTask);

            // then
            actualTimeServiceException.Should().BeEquivalentTo(
                expectedTimeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTimeAsync(It.IsAny<Time>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
