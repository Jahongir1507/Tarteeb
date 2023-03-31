﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Services.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoresController : RESTFulController
    {
        private readonly IScoreService scoreService;

        public ScoresController(IScoreService scoreService) =>
            this.scoreService = scoreService;

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
