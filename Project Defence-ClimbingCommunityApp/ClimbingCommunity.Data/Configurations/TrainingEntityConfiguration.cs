namespace ClimbingCommunity.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ClimbingCommunity.Data.Models;

    public class TrainingEntityConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder.Property(t => t.isActive)
                .HasDefaultValue(true);

        }


    }
}
