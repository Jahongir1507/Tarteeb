//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public class ScoreService : IScoreService
    {
        private readonly IStorageBroker storageBroker;
        public ScoreService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public ValueTask<Score> RetrieveScoreByIdAsync(Guid id) =>
            throw new NotImplementedException();
    }
}
