//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Runtime.CompilerServices;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations
{
    public class UserSecurityService: IUserSecurityService
    {
        private readonly ITokenBroker tokenBroker;

        public UserSecurityService(ITokenBroker tokenBroker) =>
            this.tokenBroker = tokenBroker;        

        public string CreateToken(User user) =>
            throw new NotImplementedException();          
    }
}