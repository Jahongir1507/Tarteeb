//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Services.Foundations.Scores;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoresController : RESTFulController
    {
        private readonly IScoreService scoreService;

        public ScoresController(IScoreService scoreService) =>
            this.scoreService = scoreService;

        [HttpPost]
        public async ValueTask<ActionResult<Score>> PostScoreAsync(Score score)
        {
            try
            {
                return await this.scoreService.AddScoreAsync(score);
            }
            catch (ScoreValidationException scoreValidationException)
            {
                return BadRequest(scoreValidationException.InnerException);
            }
            catch (ScoreDependencyValidationException scoreDependencyValidationException)
                when (scoreDependencyValidationException.InnerException is AlreadyExistsScoreException)
            {
                return Conflict(scoreDependencyValidationException.InnerException);
            }
            catch (ScoreDependencyValidationException scoreDependencyValidationException)
            {
                return BadRequest(scoreDependencyValidationException.InnerException);
            }
            catch (ScoreDependencyException scoreDependencyException)
            {
                return InternalServerError(scoreDependencyException.InnerException);
            }
            catch (ScoreServiceException scoreServiceException)
            {
                return InternalServerError(scoreServiceException.InnerException);
            }
        }

        [HttpDelete("{scoreId}")]
        public async ValueTask<ActionResult<Score>> DeleteScoreByIdAsync(Guid scoreId)
        {
            try
            {
                Score deletedScore =
                    await this.scoreService.RemoveScoreByIdAsync(scoreId);

                return Ok(deletedScore);
            }
            catch (ScoreValidationException scoreValidationException)
                when (scoreValidationException.InnerException is NotFoundScoreException)
            {
                return NotFound(scoreValidationException.InnerException);
            }
            catch (ScoreValidationException scoreValidationException)
            {
                return BadRequest(scoreValidationException.InnerException);
            }
            catch (ScoreDependencyValidationException scoreDependencyValidationException)
                when (scoreDependencyValidationException.InnerException is LockedScoreException)
            {
                return Locked(scoreDependencyValidationException.InnerException);
            }
            catch (ScoreDependencyValidationException scoreDependencyValidationException)
            {
                return BadRequest(scoreDependencyValidationException.InnerException);
            }
            catch (ScoreDependencyException scoreDependencyException)
            {
                return InternalServerError(scoreDependencyException.InnerException);
            }
            catch (ScoreServiceException scoreServiceException)
            {
                return InternalServerError(scoreServiceException.InnerException);
            }
        }
    }
}
