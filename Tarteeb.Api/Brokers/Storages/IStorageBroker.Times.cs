//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Time> DeleteTimeAsync(Time time);
    }
}
