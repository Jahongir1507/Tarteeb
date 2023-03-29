//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions;
using Tarteeb.Api.Models.Foundations.Users;

namespace Tarteeb.Api.Services.Foundations.TimeSlots
{
    public partial class TimeService
    {
        private void ValidateTimeOnModify(Time time)
        {
            ValidateTimeNotNull(time);

            Validate(
                (Rule: IsInvalid(time.Id), nameof(Time.Id)),
                (Rule: IsInvalid(time.HoursWorked), nameof(Time.HoursWorked)),
                (Rule: IsInvalid(time.Comment), nameof(Time.Comment)),
                (Rule: IsInvalid(time.UserId), nameof(Time.UserId)),
                (Rule: IsInvalid(time.TicketId), nameof(Time.Ticket)),
                (Rule: IsInvalid(time.CreatedDate), nameof(Time.CreatedDate)),
                (Rule: IsInvalid(time.UpdatedDate), nameof(Time.UpdatedDate)),
                (Rule: IsNotRecent(time.UpdatedDate), nameof(Time.UpdatedDate)),
                (Rule: IsInvalid(time.Ticket), nameof(Time.Ticket)),
                (Rule: IsInvalid(time.User), nameof(Time.User)),

                (Rule: IsSame(
                        firstDate: time.UpdatedDate,
                        secondDate: time.CreatedDate,
                        secondDateName: nameof(time.CreatedDate)),

                     Parameter: nameof(time.UpdatedDate)));
        }

        private static dynamic IsInvalid(User user) => new
        {
            Condition = user == default,
            Message = "User is required"
        };

        private static dynamic IsInvalid(Ticket ticket) => new
        {
            Condition = ticket == default,
            Message = "Ticket is required"
        };

        private static dynamic IsInvalid(DateTimeOffset dates) => new
        {
            Condition = dates == default,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(decimal number) => new
        {
            Condition = number == default,
            Message = "Number is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private void ValidateAgainstStorageTimeOnModify(Time inputTime, Time storageTime)
        {
            ValidateStorageTime(storageTime, inputTime.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTime.CreatedDate,
                    secondDate: storageTime.CreatedDate,
                    secondDateName: nameof(Time.CreatedDate)),
                Parameter: nameof(Time.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputTime.UpdatedDate,
                    secondDate: storageTime.UpdatedDate,
                    secondDateName: nameof(Time.UpdatedDate)),
                Parameter: nameof(Time.UpdatedDate)));
        }

        private void ValidateStorageTime(Time maybeTime, Guid TimeId)
        {
            if (maybeTime is null)
            {
                throw new NotFoundTimeException(maybeTime.Id);
            }
        }

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };


        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent."
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateTimeNotNull(Time time)
        {
            if (time is null)
            {
                throw new NullTimeException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTimeException = new InvalidTimeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTimeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTimeException.ThrowIfContainsErrors();
        }

    }
}
