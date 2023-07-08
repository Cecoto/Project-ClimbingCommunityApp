namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class LevelEntityConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasData(GenerateClimberLevels());
        }

        private Level[] GenerateClimberLevels()
        {
            ICollection<Level> climberLevels = new HashSet<Level>();

            climberLevels.Add(new Level
            {
                Id = 1,
                Name = "Begginer",
               
            });
            climberLevels.Add(new Level
            {
                Id = 2,
                Name = "Intermediate",
               
            });
            climberLevels.Add(new Level
            {   
                Id = 3,
                Name = "Advanced",
               

            });
            climberLevels.Add(new Level
            {
                Id = 4,
                Name = "Pro",
                
            });

            return climberLevels.ToArray();
        }
    }
}
