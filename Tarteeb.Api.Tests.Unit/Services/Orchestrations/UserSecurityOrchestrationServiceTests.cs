//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Foundations.Users.Exceptions;
using Tarteeb.Api.Services.Foundations.Securities;
using Tarteeb.Api.Services.Foundations.Users;
using Tarteeb.Api.Services.Orchestrations;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<ISecurityService> securityServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserSecurityOrchestrationService userSecurityOrchestrationService;

        public UserSecurityOrchestrationServiceTests()
        {
            userServiceMock = new Mock<IUserService>();
            securityServiceMock = new Mock<ISecurityService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userSecurityOrchestrationService = new UserSecurityOrchestrationService(
                userService: userServiceMock.Object,
                securityService: securityServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> UserDependencyExceptions()
        {
            var someInnerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new UserDependencyException(someInnerException),
                new UserServiceException(someInnerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 9).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private IQueryable<User> CreateRandomUsers() =>
            CreateUserFiller().Create(count: GetRandomNumber()).AsQueryable();

        private IQueryable<User> CreateRandomUsersIncluding(User user)
        {
            List<User> users = CreateUserFiller()
                .Create(count: GetRandomNumber()).ToList();

            users.Add(user);

            return users.AsQueryable();
        }

        private User CreateRandomUser() =>
            CreateUserFiller().Create();


        private static Filler<User> CreateUserFiller()
        {
            DateTimeOffset dates = GetRandomDate();
            var filler = new Filler<User>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }

    }
}
