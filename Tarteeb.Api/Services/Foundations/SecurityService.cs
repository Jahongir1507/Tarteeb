//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations;

public class SecurityService : ISecurityService
{
    private readonly ITokenBroker tokenBroker;
    private readonly ILoggingBroker loggingBroker;

    public SecurityService(ITokenBroker tokenBroker, ILoggingBroker loggingBroker)
    {
        this.tokenBroker = tokenBroker;
        this.loggingBroker = loggingBroker;
    }

    public string CreateToken(User user) =>
        tokenBroker.GenerateJWT(user);
}
