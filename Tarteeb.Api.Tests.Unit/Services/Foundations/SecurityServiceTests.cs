﻿using Moq;
using System;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;
using Tynamix.ObjectFiller;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations
{
    public partial class SecurityServiceTests
    {
        private readonly Mock<ITokenBroker> tokenBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISecurityService securityService;


        public SecurityServiceTests()
        {
            this.tokenBrokerMock = new Mock<ITokenBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.securityService = new SecurityService(
                tokenBrokerMock.Object,
                loggingBrokerMock.Object);
        }
        private static User CreateRandomUser() =>
            CreateUserFiller(dates: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
          new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Filler<User> CreateUserFiller(DateTimeOffset dates)
        {
            var filler = new Filler<User>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
