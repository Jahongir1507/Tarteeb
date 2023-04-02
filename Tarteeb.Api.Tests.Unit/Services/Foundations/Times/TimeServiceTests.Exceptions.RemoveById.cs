﻿//=================================
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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTimeId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedTimeStorageException =
                new FailedTimeStorageException(sqlException);

            var expectedTimeDependencyException =
                new TimeDependencyException(failedTimeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Time> removeTimeByIdTask =
                this.timeService.RemoveTimeByIdAsync(someTimeId);

            TimeDependencyException actualTimeDependencyException =
                await Assert.ThrowsAsync<TimeDependencyException>(
                    removeTimeByIdTask.AsTask);

            // then
            actualTimeDependencyException.Should()
                .BeEquivalentTo(expectedTimeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTimeDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTimeId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTimeServiceException =
                new FailedTimeServiceException(serviceException);

            var expectedTimeServiceException =
                new TimeServiceException(failedTimeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Time> removeTimeByIdTask =
                this.timeService.RemoveTimeByIdAsync(someTimeId);

            TimeServiceException actualTimeServiceException =
                await Assert.ThrowsAsync<TimeServiceException>(
                    removeTimeByIdTask.AsTask);

            // then
            actualTimeServiceException.Should()
                .BeEquivalentTo(expectedTimeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
