//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Users.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate IQueryable<User> ReturningUsersFunction();

        private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
        {
            try
            {
                return returningUsersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogCritical(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userServiceException =
                new UserServiceException(exception);

            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}
