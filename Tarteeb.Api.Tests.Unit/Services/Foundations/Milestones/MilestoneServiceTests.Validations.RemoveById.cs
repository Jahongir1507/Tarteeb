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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidMilestoneId = Guid.Empty;
            var invalidMilestoneException = new InvalidMilestoneException();

            invalidMilestoneException.AddData(
                key: nameof(Milestone.Id),
                values: "Id is required");

            var expectedMilestoneValidationException = new MilestoneValidationException(
                invalidMilestoneException);

            // when
            ValueTask<Milestone> removeMilestoneByIdTask =
                this.milestoneService.RemoveMilestoneByIdAsync(invalidMilestoneId);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(
                    removeMilestoneByIdTask.AsTask);

            // then
            actualMilestoneValidationException.Should().BeEquivalentTo(
                expectedMilestoneValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMilestoneAsync(It.IsAny<Milestone>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfMilestoneIsNotFoundAndLogItAsync()
        {
            //given
            Guid randomMilestoneId = Guid.NewGuid();
            Guid inputMilestoneId = randomMilestoneId;
            Milestone noMilestone = null;
            var notFoundMilestoneException = new NotFoundMilestoneException(inputMilestoneId);

            var expectedMilestoneValidationException =
                new MilestoneValidationException(notFoundMilestoneException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noMilestone);

            //when
            ValueTask<Milestone> onRemoveMilestoneTask = this.milestoneService.RemoveMilestoneByIdAsync(inputMilestoneId);

            MilestoneValidationException actualMilestoneValidationException =
                await Assert.ThrowsAsync<MilestoneValidationException>(onRemoveMilestoneTask.AsTask);

            //then
            actualMilestoneValidationException.Should()
                 .BeEquivalentTo(expectedMilestoneValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedMilestoneValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
