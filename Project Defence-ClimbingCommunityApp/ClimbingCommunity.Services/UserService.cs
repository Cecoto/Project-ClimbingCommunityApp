namespace ClimbingCommunity.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Http;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Common;

    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly IImageService imageService;

        public UserService(IRepository repo, IImageService imageService)
        {

            this.repo = repo;
            this.imageService = imageService;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            return await repo.All<ApplicationUser>()
                 .Select(user => new UserViewModel
                 {
                     Id = user.Id,
                     FirstName = user.FirstName,
                     LastName = user.LastName,
                     Email = user.Email,
                     Age = user.Age,
                     Role = user.UserType
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllUsersEmailsAsync()
        {
            return await repo.All<ApplicationUser>()
                .Select(user => user.Email)
                .ToListAsync();
        }

        public async Task<ClimberProfileViewModel> GetClimberInfoAsync(string userId)
        {
            Climber user = await repo.GetByIdIncludingAsync<Climber>(c => c.Id == userId, c => c.ClimberSpeciality, c => c.Level);


            ClimberProfileViewModel climber = new ClimberProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePictureUrl!,
                Gender = user.Gender.ToString(),
                Id = user.Id.ToString(),
                Speciality = user.ClimberSpeciality.Name,
                ClimbingExperience = user.ClimbingExperience,
                TypeOfUser = user.UserType,
                Level = user.Level.Name,
                Age = user.Age
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
                Age = user.Age,
                ProfilePictureUrl = user.ProfilePictureUrl!,
                ClimberSpecialityId = user.ClimberSpeciality.Id,
                LevelId = user.Level.Id,
                ClimbingExperience = user.ClimbingExperience
            };

        }


        public async Task<IEnumerable<ClimberSpecialityViewModel>> GetClimberSpecialitiesForFormAsync()
        {
            return await repo.AllReadonly<ClimberSpeciality>()
                .Select(cs => new ClimberSpecialityViewModel()
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
                Id = user.Id.ToString(),
                CoachingExperience = user.CoachingExperience,
                TypeOfUser = user.UserType,
                Age = user.Age

            };
            return coach;
        }

        public async Task<UpdateCoachProfileViewModel> GetCoachInfoForUpdateAsync(string id)
        {
            Coach coach = await repo.GetByIdAsync<Coach>(id);

            return new UpdateCoachProfileViewModel()
            {
                ProfilePictureUrl = coach.ProfilePictureUrl!,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                PhoneNumber = coach.PhoneNumber,
                Age = coach.Age,
                CoachingExperience = coach.CoachingExperience
            };
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

        public async Task<ICollection<PhotoViewModel>> GetPhotosForUserAsync(string userId)
        {
            return await repo.AllReadonly<Photo>()
                 .Where(p => p.UserId == userId)
                 .Select(p => new PhotoViewModel()
                 {
                     Id = p.Id,
                     ImageUrl = p.ImageUrl,
                 })
                 .ToListAsync();
        }

        public Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return repo.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<bool> IsClimbingSpecialityIdValidByIdAsync(int climberSpecialityId)
        {
            return await repo.GetByIdAsync<ClimberSpeciality>(climberSpecialityId) != null;
        }

        public async Task<bool> IsLevelIdValidByIdAsync(int levelId)
        {
            return await repo.GetByIdAsync<Level>(levelId) != null;
        }

        public async Task<bool> IsUserExistsByIdAsync(string userId)
        {
            return await repo.GetByIdAsync<ApplicationUser>(userId) != null;
        }

        public async Task SavePhotosToUserByIdAsync(string userId, List<string> savedPhotoPaths)
        {
            var photos = savedPhotoPaths.Select(savedPhotoPath => new Photo
            {
                ImageUrl = savedPhotoPath,
                UserId = userId
            });

            await repo.AddRangeAsync<Photo>(photos);

            await repo.SaveChangesAsync();
        }

        public async Task UpdateClimberInfoAsync(string userId, UpdateClimberProfileViewModel model)
        {
            Climber climber = await repo.GetByIdIncludingAsync<Climber>(c => c.Id == userId, c => c.ClimberSpeciality, c => c.Level);

            string? photo = climber.ProfilePictureUrl;

            if (model.PhotoFile != null)
            {
                if (!string.IsNullOrEmpty(photo))
                {
                    imageService.DeletePicture(photo);
                }
                photo = await imageService.SavePictureAsync(model.PhotoFile, "ProfilePictures");
            }

            climber.FirstName = model.FirstName;
            climber.LastName = model.LastName;
            climber.PhoneNumber = model.PhoneNumber;
            climber.ProfilePictureUrl = photo;
            climber.Age = model.Age;
            climber.LevelId = model.LevelId;
            climber.ClimberSpecialityId = model.ClimberSpecialityId;
            climber.ClimbingExperience = model.ClimbingExperience;


            await repo.SaveChangesAsync();
        }

        public async Task UpdateCoachInfoAsync(string userId, UpdateCoachProfileViewModel model)
        {
            Coach coach = await repo.GetByIdAsync<Coach>(userId);

            string? photo = coach.ProfilePictureUrl;

            if (model.PhotoFile != null)
            {
                if (!string.IsNullOrEmpty(photo))
                {
                    imageService.DeletePicture(photo);
                }
                photo = await imageService.SavePictureAsync(model.PhotoFile, "ProfilePictures");
            }

            coach.FirstName = model.FirstName;
            coach.LastName = model.LastName;
            coach.PhoneNumber = model.PhoneNumber;
            coach.ProfilePictureUrl = photo;
            coach.Age = model.Age;
            coach.CoachingExperience = model.CoachingExperience;

            await repo.SaveChangesAsync();
        }
    }
}