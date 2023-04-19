//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedMilestoneServiceException = new FailedMilestoneServiceException(sqlException);

            var expectedMilestoneDependencyException =
                new MilestoneDependencyException(failedMilestoneServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMilestones()).Throws(sqlException);

            // when
            Action retrieveAllMilestoneAction = () =>
                this.milestoneService.RetrieveAllMilestones();

            MilestoneDependencyException actualMilestoneDependencyException =
                Assert.Throws<MilestoneDependencyException>(retrieveAllMilestoneAction);

            // then
            actualMilestoneDependencyException.Should().BeEquivalentTo(
                expectedMilestoneDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMilestones(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedMilestoneDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
