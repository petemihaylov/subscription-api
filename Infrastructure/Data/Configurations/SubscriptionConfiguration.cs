using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.CustomerPhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.DurationMonths)
            .IsRequired();

        builder.Property(s => s.SubscriptionDate)
            .IsRequired();

        builder.HasOne(s => s.Service)
            .WithMany()
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.CustomerPhoneNumber, s.ServiceId })
            .IsUnique();
    }
}