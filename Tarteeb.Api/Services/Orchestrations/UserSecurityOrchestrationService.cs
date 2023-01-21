using System;
using System.Security.Authentication;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Orchestrations.Model;
using Tarteeb.Api.Services.Processings;

namespace Tarteeb.Api.Services.Orchestrations
{
    public class UserSecurityOrchestrationService:IUserSecurityOrchestrationService
    {
        private readonly UserProcessingService userProcessingService;
        private readonly UserSecurityService userSecurityService;

        public UserSecurityOrchestrationService(
            UserProcessingService userProcessingService,
            UserSecurityService userSecurityService)
        {
            this.userProcessingService =
                userProcessingService;

            this.userSecurityService = 
                userSecurityService;
        }
        public UserToken LoginUser(string email, string password)
        {
            User user =
                userProcessingService.RetriveUserByCredentials(email, password);

            if (user is null)
                throw new InvalidCredentialException("Email or password incorrect. Please try again.");
            
            UserToken userToken = new UserToken();
            userToken.UserId = user.Id;
            userToken.Token = userSecurityService.CreateToken(user);

            return userToken;
        }
    }
}
