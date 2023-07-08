namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class TripTypeEntityConfiguration : IEntityTypeConfiguration<TripType>
    {
        public void Configure(EntityTypeBuilder<TripType> builder)
        {
            builder.HasData(GenerateTripTypes());
        }

        private TripType[] GenerateTripTypes()
        {
            ICollection<TripType> tripTypes = new HashSet<TripType>();

            tripTypes.Add(new TripType
            {
                Id = 1,
                Name = "Bouldering"
              
            });
            tripTypes.Add(new TripType
            {
                Id = 2,
                Name = "Deep water soloing"
            });
            tripTypes.Add(new TripType
            {
                Id = 3,
                Name = "Rope-climbing"
            });

            return tripTypes.ToArray();
        }
    }
}
