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

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(au => au.Email)
                .HasMaxLength(60);

            builder.Property(au => au.UserName)
               .HasMaxLength(60);

            builder.HasDiscriminator<string>("UserType")
                .HasValue<Climber>("Climber")
                .HasValue<Coach>("Coach")
                .HasValue("Administrator");
;
        }
    }
}
