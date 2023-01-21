using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Storages;

public class StorageBrokerUsersConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(user => user.Email)
            .IsUnique();
    }
}