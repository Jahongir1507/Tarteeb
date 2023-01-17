//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Tokens
{
    public class TokenBroker : ITokenBroker
    {
        private readonly IConfiguration configuration;

        public TokenBroker(IConfiguration configuration) =>
            this.configuration = configuration;

        public string GenerateJWT(User user)
        {
            var secretKey =
                configuration.GetSection("Jwt").GetSection("Key").Value;

            var jwtIssuer =
                configuration.GetSection("Jwt").GetSection("Issuer").Value;

            var jwtAudience =
                configuration.GetSection("Jwt").GetSection("Audience").Value;

            var convertedKeyToBytes =
                Encoding.UTF8.GetBytes(secretKey);

            var securityKey =
                new SymmetricSecurityKey(convertedKeyToBytes);

            var cridentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtAudience,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cridentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}