//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private void ValidateScoreOnAdd(Score score)
        {
            ValidateScoreNotNull(score);

            Validate(
                (Rule: IsInvalid(score.Id), Parameter: nameof(Score.Id)),
                (Rule: IsInvalid(score.EffortLink), Parameter: nameof(Score.EffortLink)),
                (Rule: IsInvalid(score.CreatedDate), Parameter: nameof(Score.CreatedDate)),
                (Rule: IsInvalid(score.UpdatedDate), Parameter: nameof(Score.UpdatedDate)),
                (Rule: IsNotRecent(score.CreatedDate), Parameter: nameof(Score.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: score.CreatedDate,
                    secondDate: score.UpdatedDate,
                    secondDateName: nameof(Score.UpdatedDate)),
                 Parameter: nameof(Score.CreatedDate)));
        }

        private void ValidateScoreNotNull(Score score)
        {
            if (score is null)
            {
                throw new NullScoreException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}."
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

        private static void ValidateStorageScoreExists(Score maybeScore, Guid scoreId)
        {
            if (maybeScore is null)
            {
                throw new NotFoundScoreException(scoreId);
            }
        }

        private void ValidateScoreId(Guid scoreId) =>
            Validate((Rule: IsInvalid(scoreId), Parameter: nameof(Score.Id)));

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidScoreException = new InvalidScoreException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidScoreException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidScoreException.ThrowIfContainsErrors();
        }
    }
}
