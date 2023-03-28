//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidScoreId = Guid.Empty;

            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.Id),
                values: "Id is required");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            //when
            ValueTask<Score> removeScoreByIdTask =
                this.scoreService.RemoveScoreByIdAsync(invalidScoreId);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(
                    removeScoreByIdTask.AsTask);

            //then
            actualScoreValidationException.Should().BeEquivalentTo(expectedScoreValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteScoreAsync(It.IsAny<Score>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
