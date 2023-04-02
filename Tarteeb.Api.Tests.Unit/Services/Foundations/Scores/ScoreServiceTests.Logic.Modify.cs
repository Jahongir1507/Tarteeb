//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldModifyScoreAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Score randomScore = CreateRandomModifyScore(randomDate);
            Score inputScore = randomScore;
            Score storageScore = inputScore.DeepClone();
            storageScore.UpdatedDate = randomScore.CreatedDate;
            Score updatedScore = inputScore;
            Score expectedScore = updatedScore.DeepClone();
            Guid inputScoreId = inputScore.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(inputScoreId))
                    .ReturnsAsync(storageScore);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateScoreAsync(inputScore))
                    .ReturnsAsync(updatedScore);

            // when
            Score actualScoreTask =
                 await this.scoreService.ModifyScoreAsync(inputScore);

            // then
            actualScoreTask.Should().BeEquivalentTo(expectedScore);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(inputScoreId), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.UpdateScoreAsync(inputScore), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}