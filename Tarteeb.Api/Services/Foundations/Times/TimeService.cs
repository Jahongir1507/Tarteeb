//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public class TimeService : ITimeService 
    {
        private readonly IStorageBroker storageBroker;

        public TimeService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;        

        public ValueTask<Time> AddTimeAsync(Time time)
        {
            throw new System.NotImplementedException();
        }
    }
}
