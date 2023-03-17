//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions;

namespace Tarteeb.Api.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationService
    {
        private static void ValidateEmailAndPassword(string email, string password)
        {
            Validate(
                (Rule: IsInvalid(email), Parameter: nameof(User.Email)),
                (Rule: IsInvalid(password), Parameter: nameof(User.Password)));
        }

        private void ValidateUserExists(User user)
        {
            if (user is null)
            {
                throw new NotFoundUserException();
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserCreadentialOrchestrationException =
                new InvalidUserCredentialOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserCreadentialOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserCreadentialOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
