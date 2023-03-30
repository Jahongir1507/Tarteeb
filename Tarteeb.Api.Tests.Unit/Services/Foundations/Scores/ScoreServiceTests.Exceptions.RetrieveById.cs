//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptionis;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedScoreStorageException =
                new FailedScoreStorageException(sqlException);

            var expectedScoreDependencyException =
                new ScoreDependencyException(failedScoreStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Score> retrieveScoreByIdTask =
                this.scoreService.RetrieveScoreByIdAsync(someId);

            ScoreDependencyException actualScoreDependencyException =
                await Assert.ThrowsAsync<ScoreDependencyException>(
                    retrieveScoreByIdTask.AsTask);

            // then 
            actualScoreDependencyException.Should().BeEquivalentTo(
                expectedScoreDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedScoreDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedScoreServiceException =
                new FailedScoreServiceException(serviceException);

            var expectedScoreServiceExcpetion =
                new ScoreServiceException(failedScoreServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            // when
            ValueTask<Score> retrieveScoreById =
            this.scoreService.RetrieveScoreByIdAsync(someId);

            ScoreServiceException actualScoreServiceException =
                await Assert.ThrowsAsync<ScoreServiceException>(retrieveScoreById.AsTask);

            // then
            actualScoreServiceException.Should().BeEquivalentTo(expectedScoreServiceExcpetion);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedScoreServiceExcpetion))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
