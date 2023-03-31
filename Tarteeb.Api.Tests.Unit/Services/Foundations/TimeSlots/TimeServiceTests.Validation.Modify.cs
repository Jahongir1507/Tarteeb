//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions;
using Xunit;
using System;
using System.IO;
using System.ComponentModel;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTimeIsNullAndLogItAsync()
        {
            // given
            Time nullTime = null;
            var nullTimeException = new NullTimeException();

            var expectedTimeValidationException =
                new TimeValidationException(nullTimeException);

            // whe
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(nullTime);

            var actualTimeValidationException =
                 await Assert.ThrowsAsync<TimeValidationException>(modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTimeAsync(It.IsAny<Time>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTimeIsInvalidAndLogItAsync(string invalidString)
        {
            // given 
            var invalidTime = new Time()
            {
                Comment = invalidString
            };

            var invalidTimeException = new InvalidTimeException();

            invalidTimeException.AddData(
                key: nameof(Time.Id),
                values: "Id is required");

            invalidTimeException.AddData(
               key: nameof(Time.HoursWorked),
               values: "Value is required");

            invalidTimeException.AddData(
               key: nameof(Time.Comment),
               values: "Comment is required");

            invalidTimeException.AddData(
               key: nameof(Time.UserId),
               values: "Id is required");

            invalidTimeException.AddData(
               key: nameof(Time.TicketId),
               values: "Id is required");

            invalidTimeException.AddData(
               key: nameof(Time.CreatedDate),
               values: "Date is required");

            invalidTimeException.AddData(
                key: nameof(Ticket.UpdatedDate),
                 "Date is required",
                "Date is not recent",
                $"Date is the same as {nameof(Time.CreatedDate)}");

            invalidTimeException.AddData(
            key: nameof(Time.User),
            values: "User is required");

            invalidTimeException.AddData(
            key: nameof(Time.Ticket),
            values: "Ticket is required");

            var expectedTimeValidationException =
                new TimeValidationException(invalidTimeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(GetRandomDateTime);

            // when
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(invalidTime);

            TimeValidationException actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.UpdateTimeAsync(It.IsAny<Time>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time invalidTime = randomTime;
            var invalidTimeException = new InvalidTimeException();

            invalidTimeException.AddData(
                key: nameof(Time.UpdatedDate),
                values: $"Date is the same as {nameof(Time.CreatedDate)}");

            var expectedTimeValidationException =
                new TimeValidationException(invalidTimeException);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(invalidTime);

            var actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(
                    modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.SelectTimeByIdAsync(invalidTime.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateNotRecentAddLogItAsync(int seconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time inputTime = randomTime;
            inputTime.UpdatedDate = randomDateTime.AddMinutes(seconds);
            var invalidTimeException = new InvalidTimeException();

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            invalidTimeException.AddData(
                key: nameof(Time.UpdatedDate),
                values: "Date is not recent");

            var expectedTimeValidationException =
                new TimeValidationException(invalidTimeException);

            // when
            ValueTask<Time> modifyTimeTask = 
                this.timeService.ModifyTimeAsync(inputTime);

            var actualTimeValidationException =
               await Assert.ThrowsAsync<TimeValidationException>(
                   modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(It.IsAny<Guid>()),Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTimeDoesNotExistAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Time randomTime = CreateRandomTime(randomDateTime);
            Time nonExistTime = randomTime;
            Time nullTime = null;

            nonExistTime.UpdatedDate = 
                randomDateTime.AddMinutes(randomMinutes);

            var notFoundTimeException = 
                new NotFoundTimeException(nonExistTime.Id);

            var expectedTimeValidationException = 
                new TimeValidationException(notFoundTimeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTimeByIdAsync(nonExistTime.Id))
                    .ReturnsAsync(nullTime);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Time> modifyTimeTask =
                this.timeService.ModifyTimeAsync(nonExistTime);

            var actualTimeValidationException =
                await Assert.ThrowsAsync<TimeValidationException>(
                    modifyTimeTask.AsTask);

            // then
            actualTimeValidationException.Should()
                .BeEquivalentTo(expectedTimeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTimeByIdAsync(nonExistTime.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTimeValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
