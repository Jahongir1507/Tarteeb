//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Moq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Services.Foundations.Scores;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public class ScoreServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerBrokerMock;
        private readonly IScoreService scoreService;

        public ScoreServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerBrokerMock = new Mock<ILoggingBroker>();

            this.scoreService = new ScoreService(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerBrokerMock.Object);
        }
    }
}

