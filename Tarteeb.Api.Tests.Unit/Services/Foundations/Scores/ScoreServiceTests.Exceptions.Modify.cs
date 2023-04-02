//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using EFxceptions.Models.Exceptions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given 
            DateTimeOffset someDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomScore(someDateTime);
            Score someScore = randomScore;
            Guid scoreId = someScore.Id;
            SqlException sqlException = CreateSqlException();

            var failedScoreStorageException =
                new FailedScoreStorageException(sqlException);

            var expectedScoreDependencyException =
                new ScoreDependencyException(failedScoreStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when 
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(someScore);

            ScoreDependencyException actualScoreDependencyException =
                await Assert.ThrowsAsync<ScoreDependencyException>(modifyScoreTask.AsTask);

            // then
            actualScoreDependencyException.Should().
                BeEquivalentTo(expectedScoreDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedScoreDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(scoreId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateScoreAsync(someScore), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomScore(randomDateTime);
            Score someScore = randomScore;
            Guid scoreId = someScore.Id;
            someScore.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedScoreException =
                new FailedScoreStorageException(databaseUpdateException);

            var expectedScoreDependencyException =
                new ScoreDependencyException(failedScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(scoreId)).ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Score> modifyScoreTask = this.scoreService.ModifyScoreAsync(someScore);

            ScoreDependencyException actualScoreDependencyException =
                await Assert.ThrowsAsync<ScoreDependencyException>(modifyScoreTask.AsTask);

            // then
            actualScoreDependencyException.Should().
                BeEquivalentTo(expectedScoreDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(scoreId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomModifyScore(randomDateTime);
            Score someScore = randomScore;
            someScore.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid scoreId = someScore.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedScoreException =
                new LockedScoreException(databaseUpdateConcurrencyException);

            var expectedScoreDependencyValidationException =
                new ScoreDependencyValidationException(lockedScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(scoreId)).
                    ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Score> modifyScoreTask = this.scoreService.ModifyScoreAsync(someScore);

            ScoreDependencyValidationException actualScoreDependencyValidationException =
                await Assert.ThrowsAsync<ScoreDependencyValidationException>(modifyScoreTask.AsTask);

            // then 
            actualScoreDependencyValidationException.Should()
                .BeEquivalentTo(expectedScoreDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(scoreId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given 
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomScore(randomDateTime);
            Score someScore = randomScore;
            someScore.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedScoreException =
                new FailedScoreServiceException(serviceException);

            var expectedScoreServiceException =
                new ScoreServiceException(failedScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(someScore.Id)).
                    ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(someScore);

            ScoreServiceException actualScoreServiceException =
                await Assert.ThrowsAsync<ScoreServiceException>(
                    modifyScoreTask.AsTask);

            // then
            actualScoreServiceException.Should().
                BeEquivalentTo(expectedScoreServiceException);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectScoreByIdAsync(someScore.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            Score foreignKeyConflictedScore = CreateRandomScore();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidScoreReferenceException =
                new InvalidScoreReferenceException(foreignKeyConstraintConflictException);

            var expectedScoreDependencyValidationException =
                new ScoreDependencyValidationException(invalidScoreReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(foreignKeyConflictedScore);

            ScoreDependencyValidationException actualScoreDependencyValidationsException =
                await Assert.ThrowsAsync<ScoreDependencyValidationException>(
                    modifyScoreTask.AsTask);

            //then
            actualScoreDependencyValidationsException.Should().BeEquivalentTo(
                expectedScoreDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(foreignKeyConflictedScore.Id),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateScoreAsync(foreignKeyConflictedScore),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}