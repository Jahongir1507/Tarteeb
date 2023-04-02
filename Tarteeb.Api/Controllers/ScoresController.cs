//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
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

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Score>> GetAllScores()
        {
            try
            {
                IQueryable<Score> allScores = this.scoreService.RetrieveAllScores();

                return Ok(allScores);
            }
            catch (ScoreDependencyException scoreDependencyException)
            {
                return InternalServerError(scoreDependencyException);
            }
            catch (ScoreServiceException scoreServiceException)
            {
                return InternalServerError(scoreServiceException);
            }
        }

        [HttpGet("{scoreId}")]
        public async ValueTask<ActionResult<Score>> GetScoreByIdAsync(Guid scoreId)
        {
            try
            {
                return await this.scoreService.RetrieveScoreByIdAsync(scoreId);
            }
            catch (ScoreDependencyException scoreDependencyException)
            {
                return InternalServerError(scoreDependencyException.InnerException);
            }
            catch (ScoreValidationException scoreValidationException)
                when (scoreValidationException.InnerException is InvalidScoreException)
            {
                return BadRequest(scoreValidationException.InnerException);
            }
            catch (ScoreValidationException scoreValidationException)
                when (scoreValidationException.InnerException is NotFoundScoreException)
            {
                return NotFound(scoreValidationException.InnerException);
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
