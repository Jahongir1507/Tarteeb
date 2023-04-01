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
        public async Task ShouldRetrieveScoreByIdAsync()
        {
            // given
            Guid randomScoreId = Guid.NewGuid();
            Guid inputScoreId = randomScoreId;
            Score randomScore = CreateRandomScore();
            Score storageScore = randomScore;
            Score expectedScore = storageScore.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(inputScoreId)).ReturnsAsync(storageScore);

            // when
            Score actualScore = await this.scoreService.RetrieveScoreByIdAsync(inputScoreId);

            // then 
            actualScore.Should().BeEquivalentTo(expectedScore);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(inputScoreId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
