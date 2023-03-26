using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Scores;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Score> Scores { get; set; }
    }
}
