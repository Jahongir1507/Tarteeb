using System;
using Tarteeb.Api.Models.Users.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Processings
{
    public partial class SecurityProcessingService
    {
        private delegate string ReturningTokenFunction();
        
        private string TryCatch(ReturningTokenFunction returningTokenFunction)
        {
            try
            {
                return returningTokenFunction();
            }
            catch(Exception exception)
            {
                var failedUserServiceException = 
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}