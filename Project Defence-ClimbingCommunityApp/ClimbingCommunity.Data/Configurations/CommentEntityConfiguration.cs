namespace ClimbingCommunity.Data.Configurations
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(ct => ct.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.isActive)
                .HasDefaultValue(true);
            
            builder
                .HasOne(c=>c.ClimbingTrip)
                .WithMany(ct=>ct.Comments)
                .HasForeignKey(c=>c.ClimbingTripId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c=>c.Training)
                .WithMany(ct=>ct.Comments)
                .HasForeignKey(c=>c.TrainingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c=>c.Author)
                .WithMany(ct=>ct.Comments)
                .HasForeignKey(c=>c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
