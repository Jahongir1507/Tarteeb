//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.TimeSlots
{
    public interface ITimeService
    {
        ValueTask<Time> ModifyTimeAsync(Time time);
    }
}
