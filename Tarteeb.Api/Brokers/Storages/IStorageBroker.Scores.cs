//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Score> InsertScoreAsync(Score score);
        IQueryable<Score> SelectAllScores();
        ValueTask<Score> UpdateScoreAsync(Score score);
        ValueTask<Score> DeleteScoreAsync(Score score);       

    }
}