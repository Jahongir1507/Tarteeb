//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Score> Scores { get; set; }

        public async ValueTask<Score> InsertScoreAsync(Score score) =>
            await InsertAsync(score);

        public IQueryable<Score> SelectAllScores() =>
            SelectAll<Score>();

        public async ValueTask<Score> SelectScoreByIdAsync(Guid id) =>
           await SelectAsync<Score>(id);

        public async ValueTask<Score> UpdateScoreAsync(Score score) =>
            await UpdateAsync(score);

        public async ValueTask<Score> DeleteScoreAsync(Score score) =>
            await DeleteAsync(score);
    }
}
