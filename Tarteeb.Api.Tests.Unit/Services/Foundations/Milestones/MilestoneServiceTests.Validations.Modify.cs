//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

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

    }
}
