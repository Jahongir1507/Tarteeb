using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tarteeb.Api.Brokers.Tokens
{
    public class JWTOptionsModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        [NotMapped]
        public string SecretKey { get => Environment.GetEnvironmentVariable("SECRET_KEY"); }
    }
}
