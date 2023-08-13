namespace ClimbingCommunity.Data.Configurations
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        //public ApplicationUserEntityConfiguration(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

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

            //builder.HasData(GenerateUsers());
        }
        //private async Task<ApplicationUser[]> GenerateUsers()
        //{
        //    ICollection<ApplicationUser> users = new HashSet<ApplicationUser>();

        //    var climber = new Climber()
        //    {
        //        Id = "5366c21a-db2f-4293-9ee4-f93fc22c2450",
        //        FirstName = "Tsvetomir",
        //        LastName = "Hristov",
        //        Age = 25,
        //        Gender = 0,
        //        ProfilePictureUrl = "/images/ProfilePictures/Default/Male.png",
        //        ClimberSpecialityId = 1,
        //        ClimbingExperience = 2,
        //        LevelId = 3,
        //        UserName = "ceco@abc.bg",
        //        NormalizedUserName = "CECO@ABC.BG",
        //        Email = "ceco@abc.bg",
        //        NormalizedEmail = "CECO@ABC.BG",
        //        PhoneNumber = "0895 10 10 10",
        //        UserType = "Climber"
        //    };

        //    await _userManager.CreateAsync(climber, "123456");
        //    users.Add(climber);

        //    var coach = new Coach()
        //    {
        //        Id = "44fb2353-f1ba-4235-82d9-0e0f7b3dfbfc",
        //        FirstName = "Dimitar",
        //        LastName = "Dimitrov",
        //        Age = 35,
        //        Gender = 0,
        //        ProfilePictureUrl = "/images/ProfilePictures/Default/Male.png",
        //        CoachingExperience = 7,
        //        UserName = "dimitar@abc.bg",
        //        NormalizedUserName = "DIMITAR@ABC.BG",
        //        Email = "dimitar@abc.bg",
        //        NormalizedEmail = "DIMITAR@ABC.BG",
        //        PhoneNumber = "0895 20 20 20",
        //        UserType = "Coach"
        //    };
        //    await _userManager.CreateAsync(coach, "123456");

        //    users.Add(coach);

        //    return users.ToArray();
        //}
    }
}
