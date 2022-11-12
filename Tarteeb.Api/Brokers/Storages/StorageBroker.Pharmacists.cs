using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Pharmacist;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker 
    {
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public async ValueTask<Pharmacist> InsertPharmacistAsync(Pharmacist pharmacist) =>
            await InsertAsync(pharmacist);
    }
}
