//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Tarteeb.Api.Models.Foundations.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Milestone someMilestone = CreateRandomMilestone();
            SqlException sqlException = CreateSqlException();
            var failedMilestoneStorageException = new FailedMilestoneStorageException(sqlException);

            var expectedMilestoneDependencyException =
                new MilestoneDependencyException(failedMilestoneStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Milestone> addMilestoneTask = this.milestoneService.AddMilestoneAsync(someMilestone);

            MilestoneDependencyException actualMilestoneDependencyException =
                await Assert.ThrowsAsync<MilestoneDependencyException>(addMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyException.Should().BeEquivalentTo(expectedMilestoneDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldTrowDependencyValidationExceptionOnAddIfDuplicateErrorOccursAndLogItAsync()
        {
            // given
            Milestone someMilestone = CreateRandomMilestone();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsMilestoneException =
                new AlreadyExistsMilestoneException(duplicateKeyException);

            var expectedMilestoneDependencyValidationException =
                new MilestoneDependencyValidationException(alreadyExistsMilestoneException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Milestone> addMilestoneTask = this.milestoneService.AddMilestoneAsync(someMilestone);

            MilestoneDependencyValidationException actualMilestoneDependencyValidationException =
                await Assert.ThrowsAsync<MilestoneDependencyValidationException>(addMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyValidationException.Should().BeEquivalentTo(
                expectedMilestoneDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedMilestoneDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertMilestoneAsync(
               It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfRefenceErrorOccursAndLogItAsync()
        {
            // given
            Milestone someMilestone = CreateRandomMilestone();
            string randomMessage = GetRandomMessage();

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(randomMessage);

            var invalidMilestoneReferenceException =
                new InvalidMilestoneReferenceException(foreignKeyConstraintConflictException);

            var expectedMilestoneDependencyValidationException =
                new MilestoneDependencyValidationException(invalidMilestoneReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Milestone> addMilestoneTask =
                this.milestoneService.AddMilestoneAsync(someMilestone);

            MilestoneDependencyValidationException actualMilestoneDependencyValidationException =
                await Assert.ThrowsAsync<MilestoneDependencyValidationException>(
                    addMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyValidationException.Should().BeEquivalentTo(
                expectedMilestoneDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Milestone someMilestone = CreateRandomMilestone();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedMilestoneException = new LockedMilestoneException(dbUpdateConcurrencyException);

            var expectedMilestoneDependencyValidationException =
                new MilestoneDependencyValidationException(lockedMilestoneException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Milestone> addMilestoneTask = this.milestoneService.AddMilestoneAsync(someMilestone);

            MilestoneDependencyValidationException actualMilestoneDependencyValidationException =
                await Assert.ThrowsAsync<MilestoneDependencyValidationException>(addMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyValidationException.Should()
                .BeEquivalentTo(expectedMilestoneDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedMilestoneDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


    }
}
