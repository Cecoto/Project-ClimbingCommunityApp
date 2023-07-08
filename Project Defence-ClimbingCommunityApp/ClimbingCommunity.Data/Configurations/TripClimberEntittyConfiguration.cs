namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class TripClimberEntittyConfiguration : IEntityTypeConfiguration<TripClimber>
    {
        public void Configure(EntityTypeBuilder<TripClimber> builder)
        {

            builder.HasKey(e => new { e.TripId, e.ClimberId });

            builder
                .HasOne<Climber>(tc => tc.Climber)
                .WithMany(c => c.ClimbingTrips)
                .HasForeignKey(tc => tc.ClimberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<ClimbingTrip>(ct => ct.ClimbingTrip)
                .WithMany(ct => ct.Climbers)
                .HasForeignKey(ct => ct.TripId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        
    }
}
