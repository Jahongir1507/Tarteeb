﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService : IScoreService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ScoreService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Score> AddScoreAsync(Score score) =>
        TryCatch(async () => {
            ValidateScoreOnAdd(score);
            var date = this.dateTimeBroker.GetCurrentDateTime();

            return await this.storageBroker.InsertScoreAsync(score);
        });

        public IQueryable<Score> RetrieveAllScores() =>
           TryCatch(() => this.storageBroker.SelectAllScores());
 
        public ValueTask<Score> RetrieveScoreByIdAsync(Guid scoreId) =>
        TryCatch(async () =>
        {
            ValidateScoreId(scoreId);

            Score maybeScore =
                await this.storageBroker.SelectScoreByIdAsync(scoreId);

            ValidateStorageScoreExists(maybeScore, scoreId);

            return maybeScore;
        });

        public ValueTask<Score> RemoveScoreByIdAsync(Guid scoreId) =>
        TryCatch(async () =>
        {
            ValidateScoreId(scoreId);

            Score maybeScore =
                await this.storageBroker.SelectScoreByIdAsync(scoreId);

            ValidateStorageScoreExists(maybeScore, scoreId);

            return await this.storageBroker.DeleteScoreAsync(maybeScore);
        });
    }
}