//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private delegate ValueTask<Score> ReturningScoresFunction();
        private async ValueTask<Score> TryCatch(ReturningScoresFunction returningTeamFunction)
        {
            try
            {
                return await returningTeamFunction();
            }
            catch (InvalidScoreException invalidScoreException)
            {
                throw CreateAndLogValidationException(invalidScoreException);
            }
            catch(NotFoundScoreException notFoundScoreException)
            {
                throw CreateAndLogValidationException(notFoundScoreException);
            }
        }

        private ScoreValidationException CreateAndLogValidationException(Xeption exception)
        {
            var scoreValidationExpcetion = new ScoreValidationException(exception);
            this.loggingBroker.LogError(scoreValidationExpcetion);

            return scoreValidationExpcetion;
        }
    }
}
