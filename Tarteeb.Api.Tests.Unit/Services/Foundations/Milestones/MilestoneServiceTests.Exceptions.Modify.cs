//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccurredAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(someDateTime);
            Milestone someMilestone = randomMilestone;
            Guid milestoneId = someMilestone.Id;
            SqlException sqlException = CreateSqlException();

            var failedMilestoneStorageException =
                new FailedMilestoneStorageException(sqlException);

            var expectedMilestoneDependencyException =
                new MilestoneDependencyException(failedMilestoneStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Milestone> modifyMilestoneTask =
               this.milestoneService.ModifyMilestoneAsync(someMilestone);

            MilestoneDependencyException actualMilestoneDependencyException =
                await Assert.ThrowsAsync<MilestoneDependencyException>(
                     modifyMilestoneTask.AsTask);

            // then
            actualMilestoneDependencyException.Should().BeEquivalentTo(
               expectedMilestoneDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(milestoneId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMilestoneAsync(someMilestone), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
