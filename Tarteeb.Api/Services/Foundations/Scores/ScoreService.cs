//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public class ScoreService : IScoreService
    {
        public ValueTask<Score> RetrieveScoreByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
