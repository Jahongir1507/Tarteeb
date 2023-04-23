//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Foundations.Milestones;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public async Task ShouldRemoveMilestoneByIdAsync()
        {
            //given
            Guid randomMilestoneId = Guid.NewGuid();
            Guid inputMilestoneId = randomMilestoneId;
            Milestone randomMilestone = CreateRandomMilestone();
            Milestone storageMilestone = randomMilestone;
            Milestone expectedInputMilestone = storageMilestone;
            Milestone deletedMilestone = expectedInputMilestone;
            Milestone expectedMilestone = deletedMilestone.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(inputMilestoneId)).ReturnsAsync(storageMilestone);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteMilestoneAsync(expectedInputMilestone)).ReturnsAsync(deletedMilestone);

            //when
            Milestone actualMilestone = await this.milestoneService.RemoveMilestoneByIdAsync(inputMilestoneId);

            //then
            actualMilestone.Should().BeEquivalentTo(expectedMilestone);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(inputMilestoneId), Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMilestoneAsync(expectedInputMilestone), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
