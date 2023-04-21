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
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someMilestoneId = Guid.NewGuid();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedMilestoneException =
                new LockedMilestoneException(dbUpdateConcurrencyException);

            var expectedMilestoneDependencyValidationException =
                new MilestoneDependencyValidationException(lockedMilestoneException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Milestone> removeMilestoneByIdTask =
                this.milestoneService.RemoveMilestoneByIdAsync(someMilestoneId);

            MilestoneDependencyValidationException actualMilestoneDependencyValidationException =
                await Assert.ThrowsAsync<MilestoneDependencyValidationException>(
                    removeMilestoneByIdTask.AsTask);

            // then
            actualMilestoneDependencyValidationException.Should().BeEquivalentTo(
                expectedMilestoneDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
      
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someMilestoneId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedMilestoneStorageException =
                new FailedMilestoneStorageException(sqlException);

            var expectedMilestoneDependencyException =
                new MilestoneDependencyException(failedMilestoneStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Milestone> deleteMilestoneTask =
                this.milestoneService.RemoveMilestoneByIdAsync(someMilestoneId);

            MilestoneDependencyException actualMilestoneDependencyException =
                await Assert.ThrowsAsync<MilestoneDependencyException>(
                    deleteMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyException.Should().BeEquivalentTo(
                expectedMilestoneDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someMilestoneId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedMilestoneServiceException = 
                new FailedMilestoneServiceException(serviceException);

            var expectedMilestoneServiceException =
                new MilestoneServiceException(failedMilestoneServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Milestone> removeMilestoneByIdAsync =
                this.milestoneService.RemoveMilestoneByIdAsync(someMilestoneId);

            MilestoneServiceException actualMilestoneServiceException =
                await Assert.ThrowsAsync<MilestoneServiceException>(
                    removeMilestoneByIdAsync.AsTask);

            // then
            actualMilestoneServiceException.Should().BeEquivalentTo(
                expectedMilestoneServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
