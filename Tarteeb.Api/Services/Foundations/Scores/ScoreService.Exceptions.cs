//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptionis;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private delegate ValueTask<Score> ReturningScoreFuncion();

        private async ValueTask<Score> TryCatch(ReturningScoreFuncion returningScoreFuncion)
        {
            try
            {
                return await returningScoreFuncion();
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
            var scoreValidationException =
                new ScoreValidationException(exception);

            this.loggingBroker.LogError(scoreValidationException);

            return scoreValidationException;
        }
    }
}
