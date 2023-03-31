//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService
    {
        private void ValidateTimeId(Guid timeId) =>
            Validate((Rule: IsInvalid(timeId), Parameter: nameof(Time.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

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
