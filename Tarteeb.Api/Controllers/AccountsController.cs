//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Users.Exceptions;
using Tarteeb.Api.Models.Orchestrations.UserTokens;
using Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions;
using Tarteeb.Api.Services.Orchestrations;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountsController : RESTFulController
    {
        private readonly IUserSecurityOrchestrationService userSecurityOrchestrationService;

        public AccountsController(IUserSecurityOrchestrationService userSecurityOrchestrationService) =>
            this.userSecurityOrchestrationService = userSecurityOrchestrationService;

        [HttpGet]
        public ActionResult<UserToken> Login(string email, string password)
        {
            try
            {
                return this.userSecurityOrchestrationService.CreateUserToken(email, password);
            }
            catch (UserTokenOrchestrationValidationException userTokenOrchestrationValidationException)
                when (userTokenOrchestrationValidationException.InnerException is InvalidUserException)

            {
                return BadRequest(userTokenOrchestrationValidationException.InnerException);
            }
            catch (UserTokenOrchestrationValidationException userTokenOrchestrationValidationException)
                when (userTokenOrchestrationValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userTokenOrchestrationValidationException.InnerException);
            }
            catch (UserTokenOrchestrationDependencyException userTokenOrchestrationDependencyException)
            {
                return InternalServerError(userTokenOrchestrationDependencyException.InnerException);
            }
            catch (UserTokenOrchestrationServiceException userTokenOrchestrationServiceException)
            {
                return InternalServerError(userTokenOrchestrationServiceException.InnerException);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
