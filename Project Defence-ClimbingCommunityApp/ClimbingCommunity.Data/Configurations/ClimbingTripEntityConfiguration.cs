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

            builder.HasData(GenerateClimbingTrips());

        }

        private ClimbingTrip[] GenerateClimbingTrips()
        {
            ICollection<ClimbingTrip> trips = new HashSet<ClimbingTrip>();

            trips.Add(new ClimbingTrip
            {

                Title = "First Climbing Trip",
                Destination = "France, Fontainebleau",
                Duration = 10,
                TripTypeId = 1,
                OrganizatorId = "5366c21a-db2f-4293-9ee4-f93fc22c2450",
                IsActive = true,
                PhotoUrl = "~/images/ClimbingTrips/Font.jpg"
            });
            trips.Add(new ClimbingTrip
            {
                Title = "Second Climbing Trip",
                Destination = "South Afrika, Capetown",
                Duration = 20,
                TripTypeId = 1,
                OrganizatorId = "5366c21a-db2f-4293-9ee4-f93fc22c2450",
                IsActive = true,
                PhotoUrl = "~/images/ClimbingTrips/Rocklands.webp"
            });
            trips.Add(new ClimbingTrip
            {
                Title = "Third Climbing Trip",
                Destination = "Spain, Mallorca",
                Duration = 5,
                TripTypeId = 3,
                OrganizatorId = "5366c21a-db2f-4293-9ee4-f93fc22c2450",
                IsActive = true,
                PhotoUrl = "~/images/ClimbingTrips/Mallorca.jpg"
            });

            return trips.ToArray();
        }
    }

}
