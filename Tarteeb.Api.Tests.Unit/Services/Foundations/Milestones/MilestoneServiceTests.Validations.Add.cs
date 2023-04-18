//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAync()
        {
            // given
            Milestone noMilestone = null;
            var nullMilestoneException = new NullMilestoneException();

            var expectedMilestoneValidationException =
                new MilestoneValidationException(nullMilestoneException);

            // when
            ValueTask<Milestone> addMilestoneTask =
                this.milestoneService.AddMilestoneAsync(noMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(addMilestoneTask.AsTask);

            // then
            actualMilestoneValidationException.Should().BeEquivalentTo(
                expectedMilestoneValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfMilestoneIsInvalidAndLogItAsync(
        string invalidText)
        {
            // given
            var invalidMilestone = new Milestone
            {
                Title = invalidText
            };

            var invalidMilestoneException =
                new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                key: nameof(Milestone.Id),
                values: "Id is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.Title),
                values: "Text is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.Discription),
                values: "Text is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.Deadline),
                values: "Date is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.CreatedDate),
                values: "Date is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.UpdatedDate),
                values: "Date is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.AssigneeId),
                values: "Id is required");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            // when
            ValueTask<Milestone> addMilestoneTask =
                this.milestoneService.AddMilestoneAsync(invalidMilestone);

            MilestoneValidationException actualMilestoneValidationException =
               await Assert.ThrowsAsync<MilestoneValidationException>(
                   addMilestoneTask.AsTask);

            // then
            actualMilestoneValidationException.Should().BeEquivalentTo(
                expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset anotherRandomDateTime = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(randomDateTime);
            Milestone invalidMilestone = randomMilestone;
            invalidMilestone.UpdatedDate = anotherRandomDateTime;
            var invalidMilestoneException = new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                key: nameof(Milestone.CreatedDate),
                values: $"Date is not same as {nameof(Milestone.UpdatedDate)}.");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Milestone> addMilestoneTask = this.milestoneService.AddMilestoneAsync(invalidMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(addMilestoneTask.AsTask);

            //then
            actualMilestoneValidationException.Should().BeEquivalentTo(expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(
                It.Is(SameExceptionAs(expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertMilestoneAsync(
                It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
          int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset invalidRandomDateTime = randomDateTime.AddSeconds(invalidSeconds);
            Milestone randomInvalidMilestone = CreateRandomMilestone(invalidRandomDateTime);
            Milestone invalidMilestone = randomInvalidMilestone;
            var invalidMilestoneException = new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                key: nameof(Milestone.CreatedDate),
                values: "Date is not recent");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Milestone> addMilestoneTask = this.milestoneService.AddMilestoneAsync(invalidMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(addMilestoneTask.AsTask);

            // then
            actualMilestoneValidationException.Should().BeEquivalentTo(
                expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
