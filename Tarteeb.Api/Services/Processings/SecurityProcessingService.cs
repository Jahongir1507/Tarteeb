//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Runtime.InteropServices;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;

namespace Tarteeb.Api.Services.Processings
{
    public class SecurityProcessingService : ISecurityProcessingService
    {
        private readonly IUserSecurityService userSecurityService;

        public SecurityProcessingService(IUserSecurityService userSecurityService) =>        
            this.userSecurityService = userSecurityService;        

        public string CreateToken(User user) =>
            this.userSecurityService.CreateToken(user);
    }
}