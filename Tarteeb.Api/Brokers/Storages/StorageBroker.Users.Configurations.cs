using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Storages;

public partial class StorageBroker
{
    public void ConfigureUserEmail(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(user => user.Email)
            .IsUnique();
    }
}