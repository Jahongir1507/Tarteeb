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
        private readonly IUserOrchestrationService _userOrchestrationService;
        public AccountsController(IUserOrchestrationService userOrchestrationService)=>
            this._userOrchestrationService= userOrchestrationService;

        [HttpGet("login")]
        public  IActionResult Login(string email, string password)
        {
            try
            {
                var user = _userOrchestrationService.LoginUser(email, password);
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
