//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidScoreId = Guid.Empty;
            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.Id),
                values: "Id is required.");

            var expectedValidationScoreException = new
                ScoreValidationException(invalidScoreException);

            // when 
            ValueTask<Score> retrieveScoreByIdAsync =
                this.scoreService.RetrieveScoreByIdAsync(invalidScoreId);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(retrieveScoreByIdAsync.AsTask);

            // then 
            actualScoreValidationException.Should().BeEquivalentTo(expectedValidationScoreException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationScoreException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfScoreIsNotFoundAndLogItAsync()
        {
            // given
            Guid someScoreId = Guid.NewGuid();
            Score noScore = null;

            var notFoundScoreException =
                new NotFoundScoreException(someScoreId);

            var expectedScoreValidationException =
                new ScoreValidationException(notFoundScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noScore);

            // when
            ValueTask<Score> retrieveScoreByIdTask =
                this.scoreService.RetrieveScoreByIdAsync(someScoreId);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(
                    retrieveScoreByIdTask.AsTask);

            // then
            actualScoreValidationException.Should().BeEquivalentTo(expectedScoreValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
