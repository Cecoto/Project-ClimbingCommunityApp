namespace ClimbingCommunity.Data.Configurations
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrainingClimberEntityConfiguration : IEntityTypeConfiguration<TrainingClimber>
    {
        public void Configure(EntityTypeBuilder<TrainingClimber> builder)
        {
            builder.HasKey(tc => new { tc.ClimberId, tc.TrainingId });

            builder
                .HasOne<Climber>(tc => tc.Climber)
                .WithMany(c => c.JoinedTrainings)
                .HasForeignKey(c => c.ClimberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<Training>(t => t.Training)
                .WithMany(t => t.JoinedClimbers)
                .HasForeignKey(c => c.TrainingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
