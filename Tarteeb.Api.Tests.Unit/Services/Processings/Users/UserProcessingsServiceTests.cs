//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq.Expressions;
using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Users.Exceptions;
using Tarteeb.Api.Services.Foundations.Users;
using Tarteeb.Api.Services.Processings.Users;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingsServiceTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserProcessingService userProcessingsService;

        public UserProcessingsServiceTests()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userProcessingsService = new UserProcessingService(
                userService: this.userServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
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

        private static string GetrandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    }
}
