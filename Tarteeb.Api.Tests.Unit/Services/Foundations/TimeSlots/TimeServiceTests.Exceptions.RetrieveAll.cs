//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given 
            SqlException sqlException = CreateSqlException();

            var failedTimeStorageException = new
                 FailedTimeStorageException(sqlException);

            var expectedTimeDependencyExcepton =
                new TimeDependencyException(failedTimeStorageException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllTimes()).Throws(sqlException);

            // when
            Action retriveAllTimeAction = () =>
               this.timeService.RetrieveAllTimes();

            TimeDependencyException actualTimeDependencyException =
                Assert.Throws<TimeDependencyException>(retriveAllTimeAction);

            // then
            actualTimeDependencyException.Should().BeEquivalentTo(expectedTimeDependencyExcepton);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTimes(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogCritical(It.Is(SameExceptionAs(
                   expectedTimeDependencyExcepton))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
