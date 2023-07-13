namespace ClimbingCommunity.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using WebShopDemo.Core.Data.Common;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ClimberProfileViewModel> GetClimberInfoAsync(string userId)
        {
            Climber user = await repo.GetByIdIncludingAsync<Climber>(c => c.Id == userId, c => c.ClimberSpeciality,c => c.Level);



            ClimberProfileViewModel climber = new ClimberProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePictureUrl!,
                Gender = user.Gender.ToString(),
                Id = user.Id,
                Speciality = user.ClimberSpeciality.Name,
                ClimbingExperience = user.ClimbingExperience,
                TypeOfUser = "Climber",
                Level = user.Level.Name,
                Photos = { "/images/ProfilePictures/male.png", "/images/Photos/92.5_fontainebleau.jpg" }
            };

            return climber;
        }

        public async Task<UpdateClimberProfileViewModel> GetClimberInfoForUpdateAsync(string userId)
        {
            Climber user = await repo.GetByIdIncludingAsync<Climber>(c => c.Id == userId, c => c.ClimberSpeciality, c => c.Level);

            return new UpdateClimberProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl!,
                ClimberSpecialityId = user.ClimberSpeciality.Id,
                LevelId = user.Level.Id,
                ClimbingExperience = user.ClimbingExperience
            };
            
        }
      

        public async Task<IEnumerable<ClimberSpecialityViewModel>> GetClimberSpecialitiesForFormAsync()
        {
            return await repo.AllReadonly<ClimberSpeciality>()
                .Select(cs=> new ClimberSpecialityViewModel()
                {
                    Id = cs.Id,
                    Name = cs.Name,
                })
                .ToListAsync();
        }

        public async Task<CoachProfileViewModel> GetCoachInfoAsync(string userId)
        {
            Coach user = await repo.GetByIdAsync<Coach>(userId);

            CoachProfileViewModel coach = new CoachProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePictureUrl!,
                Gender = user.Gender.ToString(),
                Id = user.Id,
                CoachingExperience = user.CoachingExperience,
                TypeOfUser = user.UserType,
                Photos = { "/images/Photos/training241.1-1024x684.webp", "/images/Photos/coaching.webp" }
            };
            return coach;
        }

        public async Task<IEnumerable<ClimberLevelViewModel>> GetLevelsForFormAsync()
        {
            return await repo.AllReadonly<Level>()
                .Select(cs => new ClimberLevelViewModel()
                {
                    Id = cs.Id,
                    Name = cs.Name,
                })
                .ToListAsync();
        }

        public async Task UpdateClimberInfoAsync(string userId, UpdateClimberProfileViewModel model)
        {
            Climber climber = await repo.GetByIdIncludingAsync<Climber>(c => c.Id == userId, c => c.ClimberSpeciality, c => c.Level);

            climber.FirstName = model.FirstName;
            climber.LastName = model.LastName;
            climber.PhoneNumber = model.PhoneNumber;
            climber.ProfilePictureUrl = model.ProfilePictureUrl;
            climber.Email = model.Email;
            climber.LevelId = model.LevelId;
            climber.ClimberSpecialityId = model.ClimberSpecialityId;
            climber.ClimbingExperience = model.ClimbingExperience;
        
            
            await repo.SaveChangesAsync();
        }

    }
}