//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using System.Threading.Tasks;
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
    }
}
