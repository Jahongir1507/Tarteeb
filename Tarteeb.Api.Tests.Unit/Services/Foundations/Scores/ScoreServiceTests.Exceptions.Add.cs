//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using FluentAssertions;
using EFxceptions.Models.Exceptions;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
using Tarteeb.Api.Models.Foundations.Teams;

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

        [Fact]
        public async Task ShouldTrowDependencyValidationExceptionOnAddIfDuplicateErrorOccursAndLogItAsync()
        {
            // given
            Score someScore = CreateRandomScore();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsScoreException =
                new AlreadyExistsScoreException(duplicateKeyException);

            var expectedScoreDependencyValidationException =
                new ScoreDependencyValidationException(alreadyExistsScoreException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Score> addScoreTask = this.scoreService.AddScoreAsync(someScore);

            ScoreDependencyValidationException actualScoreDependencyValidationException =
                await Assert.ThrowsAsync<ScoreDependencyValidationException>(addScoreTask.AsTask);

            // then
            actualScoreDependencyValidationException.Should().BeEquivalentTo(
                expectedScoreDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedScoreDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertScoreAsync(
               It.IsAny<Score>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
