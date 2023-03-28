//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Moq;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Services.Foundations.Scores;
using Tynamix.ObjectFiller;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IScoreService scoreService;
        public ScoreServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            
            this.scoreService = new ScoreService(
                storageBroker: storageBrokerMock.Object);
        }

        private DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Score CreateRandomScore() =>
            CreateScoreFiller(GetRandomDateTime()).Create();

        private Filler<Score> CreateScoreFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Score>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
