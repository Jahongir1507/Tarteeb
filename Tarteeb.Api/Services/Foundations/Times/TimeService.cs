//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public class TimeService:ITimeService
    {
        IStorageBroker storageBroker;

        public TimeService(IStorageBroker storageBroker)=>
            this.storageBroker = storageBroker;

        public IQueryable<Time> RetrieveAllTimes()=>
            throw new System.NotImplementedException();
    }
}
