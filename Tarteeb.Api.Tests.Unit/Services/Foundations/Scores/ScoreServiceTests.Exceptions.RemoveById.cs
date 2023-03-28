//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Xunit;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using FluentAssertions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Guid someScoreId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedScoreException =
                new LockedScoreException(databaseUpdateConcurrencyException);

            var expectedScoreDependencyValidationException =
                new ScoreDependencyValidationException(lockedScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            //when
            ValueTask<Score> removeScoreByIdAsync =
                this.scoreService.RemoveScoreByIdAsync(someScoreId);

            ScoreDependencyValidationException actualScoreDependencyValidationException =
                await Assert.ThrowsAsync<ScoreDependencyValidationException>(
                    removeScoreByIdAsync.AsTask);

            //then
            actualScoreDependencyValidationException.Should().BeEquivalentTo(
                expectedScoreDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteScoreAsync(It.IsAny<Score>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
