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
            Guid ScoreId = someScore.Id;
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
                broker.SelectScoreByIdAsync(ScoreId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateScoreAsync(someScore), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


    }
}

