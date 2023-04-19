//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using System.Linq;
using Tarteeb.Api.Models.Foundations.Milestones;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllMilestones()
        {
            //given
            IQueryable<Milestone> randomMilestones = CreateRandomMilestones();
            IQueryable<Milestone> storageMilestones = randomMilestones;
            IQueryable<Milestone> expectedMilestones = storageMilestones;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMilestones()).Returns(storageMilestones);

            //when
            IQueryable<Milestone> actualMilestones =
                this.milestoneService.RetrieveAllMilestones();

            //then
            actualMilestones.Should().BeEquivalentTo(expectedMilestones);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMilestones(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
