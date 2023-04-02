//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Data;
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
                (Rule: IsInvalid(score.Id), nameof(Score.Id)),
                (Rule: IsInvalid(score.Grade), nameof(Score.Grade)),
                (Rule: IsInvalid(score.Weight), nameof(Score.Weight)),
                (Rule: IsInvalid(score.TicketId), nameof(Score.TicketId)),
                (Rule: IsInvalid(score.UserId), nameof(Score.UserId)),
                (Rule: IsInvalid(score.CreatedDate), nameof(Score.CreatedDate)),
                (Rule: IsInvalid(score.UpdatedDate), nameof(Score.UpdatedDate)),
                (Rule: IsNotRecent(score.UpdatedDate), nameof(Score.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: score.UpdatedDate,
                    secondDate: score.CreatedDate,
                    secondDateName: nameof(Score.CreatedDate)),
                    Parameter: nameof(Score.UpdatedDate)));
        }

        private void ValidateScoreOnModify(Score score)
        {
            ValidateScoreNotNull(score);
            Validate(
                (Rule: IsInvalid(score.Id), nameof(Score.Id)),
                (Rule: IsInvalid(score.Grade), nameof(Score.Grade)),
                (Rule: IsInvalid(score.Weight), nameof(Score.Weight)),
                (Rule: IsInvalid(score.TicketId), nameof(Score.TicketId)),
                (Rule: IsInvalid(score.UserId), nameof(Score.UserId)),
                (Rule: IsInvalid(score.CreatedDate), nameof(Score.CreatedDate)),
                (Rule: IsInvalid(score.UpdatedDate), nameof(Score.UpdatedDate)),
                (Rule: IsNotRecent(score.UpdatedDate), nameof(Score.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: score.UpdatedDate,
                    secondDate: score.CreatedDate,
                    secondDateName: nameof(Score.CreatedDate)),
                    Parameter: nameof(Score.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(int grade) => new
        {
            Condition = grade == default,
            Message = "Grade is required"
        };

        private static dynamic IsInvalid(float weight) => new
        {
            Condition = weight == default,
            Message = "Weight is required"
        };

        private static dynamic IsInvalid(string effortLink) => new
        {
            Condition = String.IsNullOrWhiteSpace(effortLink),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset dates) => new
        {
            Condition = dates == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
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
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

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

        private static void ValidateAgainstStorageScoreOnModify(Score inputScore, Score storageScore)
        {
            ValidateStorageScoreExists(storageScore, inputScore.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputScore.CreatedDate,
                    secondDate: storageScore.CreatedDate,
                    secondDateName: nameof(Score.CreatedDate)),
                Parameter: nameof(Score.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputScore.UpdatedDate,
                    secondDate: storageScore.UpdatedDate,
                    secondDateName: nameof(Score.UpdatedDate)),
                Parameter: nameof(Score.UpdatedDate)));
        }

        private void ValidateScoreId(Guid scoreId) =>
            Validate((Rule: IsInvalid(scoreId), Parameter: nameof(Score.Id)));

        private static void ValidateScoreNotNull(Score score)
        {
            if (score is null)
            {
                throw new NullScoreException();
            }
        }

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