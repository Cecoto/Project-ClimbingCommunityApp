namespace ClimbingCommunity.Data.Configurations
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ClimbingTripEntityConfiguration : IEntityTypeConfiguration<ClimbingTrip>
    {
        public void Configure(EntityTypeBuilder<ClimbingTrip> builder)
        {
            builder
                .Property(ct => ct.CreatedOn)
                .HasDefaultValueSql("GETDATE()");
            builder
                .Property(ct => ct.IsActive)
                .HasDefaultValue(true);
        }
    }

}
