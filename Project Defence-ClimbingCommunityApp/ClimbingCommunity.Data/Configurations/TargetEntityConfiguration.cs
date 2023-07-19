namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class TargetEntityConfiguration : IEntityTypeConfiguration<Target>
    {
        public void Configure(EntityTypeBuilder<Target> builder)
        {
            builder.HasData(GenerateTargets());
        }
        private Target[] GenerateTargets()
        {
            ICollection<Target> targets = new HashSet<Target>();

            targets.Add(new Target
            {
                Id = 1,
                Name = "Endurence"
            });
            targets.Add(new Target
            {
                Id = 2,
                Name = "Strength"
            });
            targets.Add(new Target
            {
                Id = 3,
                Name = "Power-Endurance"
            });
            targets.Add(new Target
            {
                Id = 4,
                Name = "Conditioning"
            });
            targets.Add(new Target
            {
                Id = 5,
                Name = "General fitness"
            });

            return targets.ToArray();
        }
    }
}
