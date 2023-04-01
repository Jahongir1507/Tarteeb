using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedScoreStorageException =
                new FailedScoreStorageException(sqlException);

            var expectedScoreDependencyException =
                new ScoreDependencyException(failedScoreStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllScores()).Throws(sqlException);

            // when
            Action retrieveAllScoresAction = () =>
                this.scoreService.RetrieveAllScores();

            ScoreDependencyException actualScoreDependencyException =
                Assert.Throws<ScoreDependencyException>(retrieveAllScoresAction);

            // then
            actualScoreDependencyException.Should().BeEquivalentTo(
                expectedScoreDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllScores(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedScoreDependencyException))), Times.Once);

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

            var failedScoreServiceException =
                new FailedScoreServiceException(serviceException);

            var expectedScoreServiceException =
                new ScoreServiceException(failedScoreServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllScores()).Throws(serviceException);

            // when
            Action retrieveAllScoreAction = () =>
                this.scoreService.RetrieveAllScores();

            ScoreServiceException actualScoreServiceException =
                Assert.Throws<ScoreServiceException>(retrieveAllScoreAction);

            // then
            actualScoreServiceException.Should().BeEquivalentTo(expectedScoreServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllScores(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
