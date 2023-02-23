using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Processings.Users;
using Xeptions;

namespace Tarteeb.Api.Services.Processings.Users
{
    public partial class UserProcessingService
    {
        private delegate ValueTask<User> ReturningUserFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
        }

        private UserProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var userProcessingValidationException =
                new UserProcessingValidationException(exception);

            this.loggingBroker.LogError(userProcessingValidationException);

            return userProcessingValidationException;
        }
    }
}
