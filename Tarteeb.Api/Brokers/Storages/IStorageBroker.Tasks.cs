//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================



using System.Threading.Tasks;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker<T> where T : class
    {
        ValueTask<T> InsertTaskAsync(T entity);
    }
}
