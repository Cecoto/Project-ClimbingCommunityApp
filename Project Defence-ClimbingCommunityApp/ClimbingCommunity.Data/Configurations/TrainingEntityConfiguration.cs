namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class TrainingEntityConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder
               .Property(ct => ct.CreatedOn)
               .HasDefaultValueSql("GETDATE()");

            builder
                .Property(t => t.isActive)
                .HasDefaultValue(true);

            builder.HasData(GenerateTrainings());
        }

        private Training[] GenerateTrainings()
        {
            ICollection<Training> trainings = new HashSet<Training>();

            trainings.Add(new Training
            {
                Id = new Guid("4F9E7B2F-C085-4FEA-B064-3EFBBF6BEAB2"),
                Title = "First training",
                Duration = 2,
                Location = "Bulgaria, Sofia",
                CoachId ="3c0bd0ac-9f64-4444-ab43-a31a1886462b",
                Price = 25.00m,
                TargetId = 2,
                PhotoUrl = "~/images/Traingings/Sofia.jpg"
            });
            trainings.Add(new Training
            {
                Id = new Guid("558D2F08-7CBD-4B95-A661-CD6C8320CF35"),
                Title = "Second training",
                Duration = 3,
                Location = "Austria, Innsbruck",
                CoachId = "3c0bd0ac-9f64-4444-ab43-a31a1886462b",
                Price = 25.00m,
                TargetId = 2,
                PhotoUrl = "~/images/Traingings/Innsbruck.jpg"
            });
            trainings.Add(new Training
            {
                Id = new Guid("7E614D87-FE30-40D2-9214-4AEA1CE7E98F"),
                Title = "Third training",
                Duration = 1,
                Location = "Spain, Madrid",
                CoachId = "749a78a0-ce39-4a27-b9f7-e83ec7642768",
                Price = 25.00m,
                TargetId = 2,
                PhotoUrl = "~/images/Traingings/Madrid.jpg"
            });

            return trainings.ToArray();
        }
    }
}
