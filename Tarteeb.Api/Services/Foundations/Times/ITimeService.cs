//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public interface ITimeService
    {
        IQueryable<Time> RetrieveAllTimes();
    }
}
