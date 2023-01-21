using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Threading.Tasks;
using Tarteeb.Api.Services.Orchestrations;
using Tarteeb.Api.Services.Orchestrations.Model;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : RESTFulController
    {
        private readonly IUserSecurityOrchestrationService userSecurityOrchestrationService;
        public AccountsController(IUserSecurityOrchestrationService userSecurityOrchestrationService)=>
            this.userSecurityOrchestrationService= userSecurityOrchestrationService;

        [HttpGet("login")]
        public  async ValueTask<ActionResult<UserToken>> Login(string email, string password)
        {
            try
            {
                var user = userSecurityOrchestrationService.LoginUser(email, password);
                return Ok(user);
            }
            catch (InvalidCredentialException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
