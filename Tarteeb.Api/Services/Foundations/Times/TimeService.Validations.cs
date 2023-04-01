﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Data;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService
    {
        private void ValidateTimeOnAdd(Time time)
        {
            ValidateTimeNotNull(time);

            Validate(
                (Rule: IsInvalid(time.Id), Parameter: nameof(Time.Id)),
                (Rule: IsInvalid(time.HoursWorked), Parameter: nameof(Time.HoursWorked)),
                (Rule: IsInvalid(time.CreatedDate), Parameter: nameof(Time.CreatedDate)),
                (Rule: IsInvalid(time.UpdatedDate), Parameter: nameof(Time.UpdatedDate)),
                (Rule: IsNotRecent(time.CreatedDate), Parameter: nameof(Time.CreatedDate)),
              
                (Rule: IsNotSame(
                    firstDate: time.CreatedDate,
                    secondDate: time.UpdatedDate,
                    secondDateName: nameof(Time.UpdatedDate)),

                    Parameter: nameof(Time.CreatedDate)));
        }

        private void ValidateTimeId(Guid timeId) =>
            Validate((Rule: IsInvalid(timeId), Parameter: nameof(Time.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName
            ) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };
         private static dynamic IsInvalid(decimal number) => new
         {
             Condition = number is 0,
             Message = "Value is required"
         };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }


        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private void ValidateTimeNotNull(Time time)
        {
            if (time is null)
            {
                throw new NullTimeException();
            }
        }

        private static void ValidateStorageTimeExists(Time maybeTime, Guid timeId)
        {
            if (maybeTime is null)
            {
                throw new NotFoundTimeException(timeId);
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
