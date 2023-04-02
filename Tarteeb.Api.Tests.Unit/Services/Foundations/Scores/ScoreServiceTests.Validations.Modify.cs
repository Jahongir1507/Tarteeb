//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores;
using Xunit;
using FluentAssertions;
using Force.DeepCloner;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfScoreIsNullAndLogItAsync()
        {
            // given
            Score nullScore = null;
            var nullScoreException = new NullScoreException();

            var expectedScoreValidationException =
                new ScoreValidationException(nullScoreException);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(nullScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateScoreAsync(It.IsAny<Score>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfScoreIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given 
            var invalidScore = new Score()
            {
                EffortLink = invalidString
            };

            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.Id),
                values: "Id is required");

            invalidScoreException.AddData(
               key: nameof(Score.Grade),
               values: "Grade is required");

            invalidScoreException.AddData(
               key: nameof(Score.Weight),
               values: "Weight is required");

            invalidScoreException.AddData(
               key: nameof(Score.EffortLink),
               values: "Text is required");

            invalidScoreException.AddData(
               key: nameof(Score.TicketId),
               values: "Id is required");

            invalidScoreException.AddData(
               key: nameof(Score.UserId),
               values: "Id is required");

            invalidScoreException.AddData(
               key: nameof(Score.CreatedDate),
               values: "Date is required");

            invalidScoreException.AddData(
                key: nameof(Score.UpdatedDate),
                 "Date is required",
                "Date is not recent",
                $"Date is the same as {nameof(Score.CreatedDate)}");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(GetRandomDateTime);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(invalidScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs
                    (expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.UpdateScoreAsync(It.IsAny<Score>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateScore = GetRandomDateTime();
            Score randomScore = CreateRandomScore(randomDateScore);
            Score invalidScore = randomScore;
            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.UpdatedDate),
                values: $"Date is the same as {nameof(Score.CreatedDate)}");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateScore);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(invalidScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(invalidScore.Id), Times.Never());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int seconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomScore(randomDateTime);
            Score inputScore = randomScore;
            inputScore.UpdatedDate = randomDateTime.AddMinutes(seconds);
            var invalidScoreException = new InvalidScoreException();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            invalidScoreException.AddData(
                key: nameof(Score.UpdatedDate),
                values: "Date is not recent");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(inputScore);

            var actualScoreValidationException =
               await Assert.ThrowsAsync<ScoreValidationException>(
                   modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfScoreDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomScore(randomDateTime);
            Score nonExistScore = randomScore;
            nonExistScore.CreatedDate = randomDateTime.AddMinutes(randomNegativMinutes);
            Score nullScore = null;

            var notFoundScoreException =
                new NotFoundScoreException(nonExistScore.Id);

            var expectedScoreValidationException =
                new ScoreValidationException(notFoundScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(nonExistScore.Id))
                    .ReturnsAsync(nullScore);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(nonExistScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(
                    modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(nonExistScore.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomModifyScore(randomDateTime);
            Score invalidScore = randomScore.DeepClone();
            Score storageScore = invalidScore.DeepClone();
            storageScore.CreatedDate = storageScore.CreatedDate.AddMinutes(randomMinutes);
            storageScore.UpdatedDate = storageScore.UpdatedDate.AddMinutes(randomMinutes);
            var invalidScoreException = new InvalidScoreException();
            Guid ScoreId = invalidScore.Id;

            invalidScoreException.AddData(
                key: nameof(Score.CreatedDate),
                values: $"Date is not the same as {nameof(Score.CreatedDate)}");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectScoreByIdAsync(ScoreId)).ReturnsAsync(storageScore);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when 
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(invalidScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should().
                BeEquivalentTo(expectedScoreValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(invalidScore.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Score randomScore = CreateRandomModifyScore(randomDateTime);
            Score invalidScore = randomScore;
            Score storageScore = randomScore.DeepClone();
            invalidScore.UpdatedDate = storageScore.UpdatedDate;
            var invalidScoreException = new InvalidScoreException();

            invalidScoreException.AddData(
                key: nameof(Score.UpdatedDate),
                values: $"Date is the same as {nameof(Score.UpdatedDate)}");

            var expectedScoreValidationException =
                new ScoreValidationException(invalidScoreException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectScoreByIdAsync(invalidScore.Id)).
                    ReturnsAsync(storageScore);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(invalidScore);

            var actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(
                    modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectScoreByIdAsync(invalidScore.Id), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}