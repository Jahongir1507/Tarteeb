//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================



using System.Reflection.Emit;
using System.Threading.Tasks;
using Local = Tarteeb.Api.Models.Tasks;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker 
    {
        public ValueTask<Local.Task> InsertTaskAsync(Local.Task student);
    }
}
