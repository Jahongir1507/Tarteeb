﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Time> InsertTimeAsync(Time time);
        ValueTask<Time> SelectTimeByIdAsync(Guid id);
        IQueryable<Time> SelectAllTimes();
        ValueTask<Time> UpdateTimeAsync(Time time);
        ValueTask<Time> DeleteTimeAsync(Time time);
    }
}
