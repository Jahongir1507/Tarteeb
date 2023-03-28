//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Xunit;
using FluentAssertions;
using Force.DeepCloner;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldRemoveScoreByIdAsync()
        {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputScoreId = randomId;
            Score randomScore = CreatRandomScore();
            Score storageScore = randomScore;
            Score expectedInputScore = storageScore;
            Score deletedScore = expectedInputScore;
            Score expectedScore = deletedScore.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(inputScoreId))
                    .ReturnsAsync(storageScore);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteScoreAsync(expectedInputScore))
                    .ReturnsAsync(deletedScore);

            //when
            Score actualScore = await this.scoreService
                .RemoveScoreByIdAsync(inputScoreId);

            //then
            actualScore.Should().BeEquivalentTo(expectedScore);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(inputScoreId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteScoreAsync(expectedInputScore), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
