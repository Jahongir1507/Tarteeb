//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Moq;
using System;
using System.Linq.Expressions;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Services.Foundations.Users;
using Tarteeb.Api.Services.Processings.Users;
using Tynamix.ObjectFiller;
using Xeptions;

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

        private static string GetrandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
