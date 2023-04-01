//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given 
            DateTimeOffset someDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(someDateTime);
            Time someTime = randomTime;
            Guid TimeId = someTime.Id;
            SqlException sqlException = CreateSqlException();

            var failedTimeStorageException =
                new FailedTimeStorageException(sqlException);

            var expectedTimeDependencyException =
                new TimeDependencyException(failedTimeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime()).Throws(sqlException);

            // when 
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(someTime);

            TimeDependencyException actualTimeDependencyException =
                await Assert.ThrowsAsync<TimeDependencyException>(modifyTimeTask.AsTask);

            // then
            actualTimeDependencyException.Should()
                 .BeEquivalentTo(expectedTimeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTimeDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(TimeId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTimeAsync(someTime), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time someTime = randomTime;
            Guid TimeId = someTime.Id;
            someTime.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedTimeException =
                new FailedTimeStorageException(databaseUpdateException);

            var expectedTimeDependencyException =
                new TimeDependencyException(failedTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(TimeId)).ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Time> modifyTimeTask = this.timeService.ModifyTimeAsync(someTime);

            TimeDependencyException actualTimeDependencyException =
                await Assert.ThrowsAsync<TimeDependencyException>(modifyTimeTask.AsTask);

            // then
            actualTimeDependencyException.Should()
                 .BeEquivalentTo(expectedTimeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(TimeId), Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomModifyTime(randomDateTime);
            Time someTime = randomTime;
            someTime.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid TimeId = someTime.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTimeException =
                new LockedTimeException(databaseUpdateConcurrencyException);

            var expectedTimeDependencyValidationException =
                new TimeDependencyValidationException(lockedTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(TimeId)).
                    ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Time> modifyTimeTask = this.timeService.ModifyTimeAsync(someTime);

            TimeDependencyValidationException actualTimeDependencyValidationException =
                await Assert.ThrowsAsync<TimeDependencyValidationException>(modifyTimeTask.AsTask);

            // then 
            actualTimeDependencyValidationException.Should()
                 .BeEquivalentTo(expectedTimeDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(TimeId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time someTime = randomTime;
            someTime.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedTimeException =
                new FailedTimeServiceException(serviceException);

            var expectedTimeServiceException =
                new TimeServiceException(failedTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(someTime.Id)).
                    ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(someTime);

            TimeServiceException actualTimeServiceException =
                await Assert.ThrowsAsync<TimeServiceException>(
                    modifyTimeTask.AsTask);

            // then
            actualTimeServiceException.Should()
                 .BeEquivalentTo(expectedTimeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(), Times.Once());

            this.storageBrokerMock.Verify(broker =>
               broker.SelectTimeByIdAsync(someTime.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
