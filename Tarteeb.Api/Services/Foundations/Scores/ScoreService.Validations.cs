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
                (Rule: IsInvalid(score.UpdatedDate), Parameter: nameof(Score.UpdatedDate))
                );
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
