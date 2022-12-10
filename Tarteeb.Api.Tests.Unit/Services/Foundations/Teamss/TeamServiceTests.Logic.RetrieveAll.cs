using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Teams;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teamss
{
    public partial class TeamServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTeams()
        {
            //given
            IQueryable<Team> randomTeams = CreateRandomTeam();
            IQueryable<Team> storageTeams = randomTeams;
            IQueryable<Team> expectedTeam = storageTeams;

            this.storageBrokerMock.Setup(broker =>
            broker.SelectAllTeams()).Returns(storageTeams);

            //when
            IQueryable<Team> actualTeam = this.teamService.RetrieveAllTeams();

            //then
            actualTeam.Should().BeEquivalentTo(expectedTeam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeams(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
