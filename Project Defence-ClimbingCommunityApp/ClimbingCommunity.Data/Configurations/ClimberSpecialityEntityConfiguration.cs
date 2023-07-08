namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class ClimberSpecialityEntityConfiguration : IEntityTypeConfiguration<ClimberSpeciality>
    {
        public void Configure(EntityTypeBuilder<ClimberSpeciality> builder)
        {
            builder.HasData(this.GenerateSpecialities());
        }

        private ClimberSpeciality[] GenerateSpecialities()
        {
            ICollection<ClimberSpeciality> climberSpecialities = new HashSet<ClimberSpeciality>();

            climberSpecialities.Add(new ClimberSpeciality
            {
                Id = 1,
                Name = "Boulderer",

            });
            climberSpecialities.Add(new ClimberSpeciality
            {
                Id = 2,
                Name = "Rope climber",    
            });
            climberSpecialities.Add(new ClimberSpeciality
            {
                Id = 3,
                Name = "Free solo climber",
            });
            climberSpecialities.Add(new ClimberSpeciality
            {
                Id = 4,
                Name = "Speed climber",
            });
            climberSpecialities.Add(new ClimberSpeciality
            {
                Id = 5,
                Name = "All rounder",
            });

            return climberSpecialities.ToArray();
        }
    }
}
