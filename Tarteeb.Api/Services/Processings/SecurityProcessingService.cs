//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Runtime.InteropServices;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;

namespace Tarteeb.Api.Services.Processings
{
    public partial class SecurityProcessingService : ISecurityProcessingService
    {
        private readonly ISecurityService securityService;
        private readonly ILoggingBroker loggingBroker;

        public SecurityProcessingService(
            ISecurityService securityService, 
            ILoggingBroker loggingBroker)
        {
            this.securityService = securityService;
            this.loggingBroker = loggingBroker;
        } 

        public string CreateToken(User user) =>
            this.securityService.CreateToken(user);
    }
}