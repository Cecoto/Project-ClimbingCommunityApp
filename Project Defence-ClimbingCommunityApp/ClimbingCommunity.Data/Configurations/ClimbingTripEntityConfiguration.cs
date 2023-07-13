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
                Id = new Guid("A1290DD3-09B5-41E0-AC6A-830A066A52AE"),
                Title = "First Climbing Trip",
                Destination = "France, Fontainebleau",
                Duration = 10,
                TripTypeId = 1,
                OrganizatorId = "AQAAAAEAACcQAAAAELC8ocjek2rGuyX/i41L/n8p0k6FYeb8iAMrd050OIN114nzsK40nsGoJYiIuiO2bg==",
                IsActive = true,
                PhotoUrl = "\"~/images/ClimbingTrips/Font.jpg\""
            });
            trips.Add(new ClimbingTrip
            {
                Id = new Guid("8BB0940C-2C84-4FBD-91C8-19ADD538D577"),
                Title = "Second Climbing Trip",
                Destination = "South Afrika, Capetown",
                Duration = 20,
                TripTypeId = 1,
                OrganizatorId = "AQAAAAEAACcQAAAAELC8ocjek2rGuyX/i41L/n8p0k6FYeb8iAMrd050OIN114nzsK40nsGoJYiIuiO2bg==",
                IsActive = true,
                PhotoUrl = "~/images/ClimbingTrips/Rocklands.webp"
            });
            trips.Add(new ClimbingTrip
            {
                Id = new Guid("43E807E1-AD36-4EEC-BAD9-9CDFB9A3E66D"),
                Title = "Third Climbing Trip",
                Destination = "Spain, Mallorca",
                Duration = 5,
                TripTypeId = 3,
                OrganizatorId = "AQAAAAEAACcQAAAAELC8ocjek2rGuyX/i41L/n8p0k6FYeb8iAMrd050OIN114nzsK40nsGoJYiIuiO2bg==",
                IsActive = true,
                PhotoUrl = "~/images/ClimbingTrips/Mallorca.jpg"
            });

            return trips.ToArray();
        }
    }

}
