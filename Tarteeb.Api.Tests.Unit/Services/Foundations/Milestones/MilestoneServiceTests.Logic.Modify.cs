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
        public async Task ShouldModifyMilestoneAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomModifyMilestone(randomDate);
            Milestone inputMilestone = randomMilestone;
            Milestone storageMilestone = inputMilestone.DeepClone();
            storageMilestone.UpdatedDate = randomMilestone.CreatedDate;
            Milestone updatedMilestone = inputMilestone;
            Milestone expectedMilestone = updatedMilestone.DeepClone();
            Guid milestoneId = inputMilestone.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMilestoneByIdAsync(milestoneId))
                    .ReturnsAsync(storageMilestone);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateMilestoneAsync(inputMilestone)).ReturnsAsync(updatedMilestone);

            // when
            Milestone actualMilestone =
                await this.milestoneService.ModifyMilestoneAsync(inputMilestone);

            // then
            actualMilestone.Should().BeEquivalentTo(expectedMilestone);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMilestoneByIdAsync(milestoneId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMilestoneAsync(inputMilestone), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
