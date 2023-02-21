using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;
using Tarteeb.Api.Services.Processings;
using Tynamix.ObjectFiller;

namespace Tarteeb.Api.Tests.Unit.Services.Processings
{
    public partial class SecurityProcessingServiceTests
    {
        private readonly Mock<ISecurityService> securityServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISecurityProcessingService securityProcessingService;

        public SecurityProcessingServiceTests()
        {
            this.securityServiceMock = new Mock<ISecurityService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.securityProcessingService = new SecurityProcessingService(
                securityServiceMock.Object,
                loggingBrokerMock.Object);
        }

        private string CreateRandomString() =>
            new MnemonicString().GetValue();

        private static User CreateRandomUser() =>
            CreateUserFiller(date: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Filler<User> CreateUserFiller(DateTimeOffset date)
        {
            var filler = new Filler<User>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;                
        }
    }
}