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
        public async Task ShouldThrowValidationExceptionOnModifyIfMilestoneIsNullAndLogItAsync()
        {
            //given
            Milestone nullMilestone = null;
            var nullMilestoneException = new NullMilestoneException();

            var expectedMilestoneValidationException =
                new MilestoneValidationException(nullMilestoneException);

            //when
            ValueTask<Milestone> onModifyMilestoneTask =
                this.milestoneService.ModifyMilestoneAsync(nullMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(onModifyMilestoneTask.AsTask);

            //then
            actualMilestoneValidationException.Should().BeEquivalentTo(expectedMilestoneValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfMilestoneIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given 
            var invalidMilestone = new Milestone
            {
                Title = invalidString
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
                key: nameof(Milestone.Deadline),
                values: "Date is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.CreatedDate),
                values: "Date is required");

            invalidMilestoneException.AddData(
                key: nameof(Milestone.UpdatedDate),
                    values: new[]
                    {
                        "Date is required",
                        "Date is not recent",
                        $"Date is the same as {nameof(Milestone.CreatedDate)}"
                    }
                );

            invalidMilestoneException.AddData(
                key: nameof(Milestone.AssigneeId),
                values: "Id is required");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Milestone> modifyMilestoneTask =
                this.milestoneService.ModifyMilestoneAsync(invalidMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(
                    modifyMilestoneTask.AsTask);

            // then
            actualMilestoneValidationException.Should()
                .BeEquivalentTo(expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(randomDateTime);
            Milestone invalidMilestone = randomMilestone;
            var invalidMilestoneException = new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                key: nameof(Milestone.UpdatedDate),
                values: $"Date is the same as {nameof(Milestone.CreatedDate)}");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Milestone> modifyMilestoneTask =
                this.milestoneService.ModifyMilestoneAsync(invalidMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(modifyMilestoneTask.AsTask);

            //then
            actualMilestoneValidationException.Should()
                  .BeEquivalentTo(expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedMilestoneValidationException))), Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(invalidMilestone.Id), Times.Never());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(dateTime);
            Milestone inputMilestone = randomMilestone;
            inputMilestone.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidMilestoneException = new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                    key: nameof(Milestone.UpdatedDate),
                    values: "Date is not recent");

            var expectedMilestoneValidationException =
                new MilestoneValidationException(invalidMilestoneException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            // when
            ValueTask<Milestone> modifyMilestoneTask =
                this.milestoneService.ModifyMilestoneAsync(inputMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(modifyMilestoneTask.AsTask);

            // then
            actualMilestoneValidationException.Should().BeEquivalentTo(expectedMilestoneValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnMofifyIsMilestoneDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(randomDateTime);
            Milestone nonExistMilestone = randomMilestone;
            nonExistMilestone.CreatedDate = randomDateTime.AddMinutes(randomNegativeMinutes);
            Milestone nullMilestone = null;
            var notFoundMilestoneException = new NotFoundMilestoneException(nonExistMilestone.Id);

            var expectedMilestoneValidationException =
                new MilestoneValidationException(notFoundMilestoneException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(nonExistMilestone.Id)).ReturnsAsync(nullMilestone);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Milestone> modifyMilestoneTask = this.milestoneService.ModifyMilestoneAsync(nonExistMilestone);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(modifyMilestoneTask.AsTask);

            //then
            actualMilestoneValidationException.Should()
                .BeEquivalentTo(expectedMilestoneValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(nonExistMilestone.Id), Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once());
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
