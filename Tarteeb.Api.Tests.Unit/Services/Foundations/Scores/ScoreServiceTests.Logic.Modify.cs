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
using Tynamix.ObjectFiller;
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
            Score randomScore = CreateRandomScore(randomDate);
            Score inputScore = randomScore;
            inputScore.UpdatedDate = randomDate.AddMinutes(1);
            Score storageScore = inputScore;
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
            Score actualScore =
                 await this.scoreService.ModifyScoreAsync(inputScore);

            // then
            actualScore.Should().BeEquivalentTo(expectedScore);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(inputScoreId), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.UpdateScoreAsync(inputScore), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

