//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Score someScore = CreateRandomScore();
            SqlException sqlException = CreateSqlException();
            var failedScoreStorageException = new FailedScoreStorageException(sqlException);

            var expectedScoreDependencyException =
                new ScoreDependencyException(failedScoreStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Score> addScoreTask = this.scoreService.AddScoreAsync(someScore);

            ScoreDependencyException actualScoreDependencyException =
                await Assert.ThrowsAsync<ScoreDependencyException>(addScoreTask.AsTask);

            // then
            actualScoreDependencyException.Should().BeEquivalentTo(expectedScoreDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedScoreDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertScoreAsync(It.IsAny<Score>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
