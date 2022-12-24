//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> AddUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);

            return await this.storageBroker.InsertUserAsync(user);
        });

        public IQueryable<User> RetrieveAllUsers() =>
        TryCatch(() => this.storageBroker.SelectAllUsers());

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);

            User maybeUser =
                await this.storageBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return maybeUser;
        });

        public ValueTask<User> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);

            User maybeUser =
                await this.storageBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return await this.storageBroker.DeleteUserAsync(maybeUser);
        });
    }
}