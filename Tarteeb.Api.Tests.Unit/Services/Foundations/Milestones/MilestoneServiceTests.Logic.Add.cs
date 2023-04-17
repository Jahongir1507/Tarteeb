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
        public async Task ShouldAddMilestoneAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTime();
            Milestone randomMilestone = CreateRandomMilestone(randomDate);
            Milestone inputMilestone = randomMilestone;
            Milestone persistedMilestone = inputMilestone;
            Milestone expectedMilestone = persistedMilestone.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertMilestoneAsync(inputMilestone)).ReturnsAsync(persistedMilestone);

            //when
            Milestone actualMilestone = await milestoneService.AddMilestoneAsync(inputMilestone);

            //then
            actualMilestone.Should().BeEquivalentTo(expectedMilestone);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMilestoneAsync(inputMilestone), Times.Once);

            storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
