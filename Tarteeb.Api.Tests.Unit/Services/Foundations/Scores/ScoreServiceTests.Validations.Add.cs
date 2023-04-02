//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Moq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores;
using Xunit;
using FluentAssertions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Score noScore = null;
            var nullScoreException = new NullScoreException();

            var expectedScoreValidationException =
                new ScoreValidationException(nullScoreException);

            // when
            ValueTask<Score> addScoreTask =
                this.scoreService.AddScoreAsync(noScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(
                    addScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfScoreIsInvalidAndLogItAsync(
          string invalidString)
        {
            // given
            var invalidScore = new Score
            {
                EffortLink = invalidString
            };

            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.Id),
                values: "Id is required");

            invalidScoreException.AddData(
                key: nameof(Score.EffortLink),
                    values: "Text is required");

            invalidScoreException.AddData(
                key: nameof(Score.CreatedDate),
                values: "Value is required");

            invalidScoreException.AddData(
                key: nameof(Score.UpdatedDate),
                values: "Value is required");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            // when
            ValueTask<Score> addScoreTask = this.scoreService.AddScoreAsync(invalidScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(addScoreTask.AsTask);

            // then
            actualScoreValidationException.Should().BeEquivalentTo(expectedScoreValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertScoreAsync(It.IsAny<Score>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
