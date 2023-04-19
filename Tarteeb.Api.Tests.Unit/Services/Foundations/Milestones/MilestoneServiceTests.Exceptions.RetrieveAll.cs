//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedMilestoneStorageException = new FailedMilestoneStorageException(sqlException);

            var expectedMilestoneDependencyException =
                new MilestoneDependencyException(failedMilestoneStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMilestones()).Throws(sqlException);

            //when
            Action retrieveAllMilestoneAction = () =>
                this.milestoneService.RetrieveAllMilestones();

            MilestoneDependencyException actualMilestoneDependencyException =
                Assert.Throws<MilestoneDependencyException>(retrieveAllMilestoneAction);
            //then
            actualMilestoneDependencyException.Should().BeEquivalentTo(expectedMilestoneDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMilestones(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMilestoneDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedMilestoneServiceException =
                new FailedMilestoneServiceException(serviceException);

            var expectedMilestoneServiceException =
                new MilestoneServiceException(failedMilestoneServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMilestones()).Throws(serviceException);

            // when
            Action retrieveAllMilestoneAction = () =>
                this.milestoneService.RetrieveAllMilestones();

            MilestoneServiceException actualMilestoneServiceException =
                Assert.Throws<MilestoneServiceException>(retrieveAllMilestoneAction);

            // then
            actualMilestoneServiceException.Should().BeEquivalentTo(expectedMilestoneServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMilestones(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
