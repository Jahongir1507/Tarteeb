//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptionis;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private void ValidateScoreId(Guid scoreId) =>
            Validate((Rule: IsInvalid(scoreId), Parameter: nameof(Score.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required."
        };
        private static void ValidateStorageScore(Score maybeScore, Guid id)
        {
            if (maybeScore is null)
            {
                throw new NotFoundScoreException(id);
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
