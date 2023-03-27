//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Score> Scores { get; set; }

        public async ValueTask<Score> InsertScoreAsync(Score score) =>
            await InsertAsync(score);
    }
}
