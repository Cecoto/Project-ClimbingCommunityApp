namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;
    using static System.Net.Mime.MediaTypeNames;


    public class TrainingService : ITrainingService
    {
        private readonly IRepository repo;
        private readonly IImageService imageService;
        public TrainingService(IRepository _repo, IImageService _imageService)
        {
            repo = _repo;
            imageService = _imageService;

        }

        public async Task CreateAsync(string organizatorId, TrainingFormViewModel model)
        {
            if (model.PhotoFile != null)
            {

                model.PhotoUrl = await imageService.SavePictureAsync(model.PhotoFile, "Trainings");
            }

            Training training = new Training()
            {
                Title = model.Title,
                Duration = model.Duration,
                Price = model.Price,
                CoachId = organizatorId,
                Location = model.Location,
                TargetId = model.TragetId,
                PhotoUrl = model.PhotoUrl!
            };

            await repo.AddAsync(training);
            await repo.SaveChangesAsync();

        }

        public async Task DeleteTrainingByIdAsync(string trainingId)
        {
            Training training = await repo.GetByIdAsync<Training>(Guid.Parse(trainingId));

            if (training != null)
            {
                training.isActive = false;
            }
            await repo.SaveChangesAsync();
        }

        public async Task EditTrainingByIdAsync(string trainingId, TrainingFormViewModel model)
        {
            Training training = await repo.GetByIdAsync<Training>(Guid.Parse(trainingId));

            string photo = training.PhotoUrl;

            if (model.PhotoFile != null)
            {
                if (!string.IsNullOrEmpty(photo))
                {
                    imageService.DeletePicture(photo);
                }
                photo = await imageService.SavePictureAsync(model.PhotoFile, "Trainings");
            }
            training.Title = model.Title;
            training.Duration = model.Duration;
            training.Price = model.Price;
            training.Location = model.Location;
            training.TargetId = model.TragetId;
            training.PhotoUrl = photo;

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<JoinedTrainingViewModel>> GetAllJoinedTrainingByUserIdAsync(string userId)
        {
            return await repo
                     .AllReadonly<Training>(t => (t.isActive == true || t.isActive == null) && t.JoinedClimbers.Any(c => c.ClimberId == userId))
                     .OrderByDescending(t => t.CreatedOn)
                     .Select(c => new JoinedTrainingViewModel()
                     {
                         Id = c.Id.ToString(),
                         Title = c.Title,
                         PhotoUrl = c.PhotoUrl,
                         Location = c.Location,
                         OrganizatorId = c.CoachId,
                         Organizator = c.Coach,
                         Duration = c.Duration,
                         Target = c.Target.Name,
                         Price = c.Price
                     }).ToListAsync();

        }

        public async Task<IEnumerable<TargetViewModel>> GetAllTargetsAsync()
        {
            return await repo.AllReadonly<Target>()
                .Select(t => new TargetViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync()
        {
            return await repo.AllReadonly<Training>(t => t.isActive == true || t.isActive == null)
                 .OrderByDescending(t => t.CreatedOn)
                 .Select(t => new TrainingViewModel()
                 {
                     Id = t.Id.ToString(),
                     Title = t.Title,
                     PhotoUrl = t.PhotoUrl,
                     Location = t.Location,
                     OrganizatorId = t.CoachId,
                     Duration = t.Duration,
                     Target = t.Target.Name,
                     isOrganizator = false,
                     Organizator = t.Coach,
                     Price = t.Price
                 }).ToListAsync();
        }

        public async Task<IEnumerable<TrainingViewModel>> GetAllTrainingsByStringAsync(string text)
        {
            return await repo.AllReadonly<Training>(t => t.isActive == true || t.isActive == null)
                .OrderByDescending(t => t.CreatedOn)
                .Where(t=>t.Title.Contains(text) ||
                t.Coach.FirstName.Contains(text) ||
                t.Coach.LastName.Contains(text) ||
                t.Target.Name.Contains(text)||
                t.Location.Contains(text))
                .Select(t => new TrainingViewModel()
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price
                }).ToListAsync();
        }

        public async Task<IEnumerable<AdminActivityViewModel>> GetAllTrainingsForAdminAsync()
        {
            return await repo.All<Training>()
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new AdminActivityViewModel()
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = ct.isActive,
                    Location = ct.Location
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainingViewModel>> GetLastThreeTrainingsAsync()
        {
            return await repo.AllReadonly<Training>(t => t.isActive == true || t.isActive == null)
               .OrderByDescending(ct => ct.CreatedOn)
               .Take(3)
               .Select(t => new TrainingViewModel()
               {
                   Id = t.Id.ToString(),
                   Title = t.Title,
                   PhotoUrl = t.PhotoUrl,
                   Location = t.Location,
                   OrganizatorId = t.CoachId,
                   Duration = t.Duration,
                   Target = t.Target.Name,
                   isOrganizator = false

               }).ToListAsync();
        }

        public async Task<IEnumerable<TrainingViewModel>> GetMyTrainingsByIdAsync(string userId)
        {
            return await repo.AllReadonly<Training>(t => t.CoachId == userId && (t.isActive == true || t.isActive == null))
                 .Select(t => new TrainingViewModel()
                 {
                     Id = t.Id.ToString(),
                     Title = t.Title,
                     PhotoUrl = t.PhotoUrl,
                     Location = t.Location,
                     Duration = t.Duration,
                     Target = t.Target.Name,
                     isOrganizator = true,
                     Price = t.Price
                 })
                 .ToListAsync();
        }

        public async Task<TrainingFormViewModel> GetTrainingForEditByIdAsync(string trainingId)
        {
            Training training = await repo.GetByIdAsync<Training>(Guid.Parse(trainingId));

            return new TrainingFormViewModel()
            {
                Title = training.Title,
                PhotoUrl = training.PhotoUrl,
                Location = training.Location,
                IsEditModel = true,
                Duration = training.Duration,
                Price = training.Price,
                OrganizatorId = training.CoachId,
                TragetId = training.TargetId
            };

        }

        public async Task<bool> IsTargetExistsByIdAsync(int tragetId)
        {
            return await repo.GetByIdAsync<Target>(tragetId) != null;
        }

        public async Task<bool> IsTrainingExistsByIdAsync(string trainingId)
        {
            return await repo.GetByIdAsync<Training>(Guid.Parse(trainingId)) != null;
        }

        public async Task<bool> IsUserOrganizatorOfTrainingByIdAsync(string userId, string trainingId)
        {
            Training training = await repo.GetByIdAsync<Training>(Guid.Parse(trainingId));

            if (training.CoachId == userId)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserParticipateInTrainingByIdAsync(string userId, string trainingId)
        {
            return await repo.GetByIdsAsync<TrainingClimber>(new object[] { userId, Guid.Parse(trainingId) }) != null;
        }

        public async Task JoinTrainingAsync(string trainingId, string userId)
        {
            await repo.AddAsync<TrainingClimber>(new TrainingClimber()
            {
                ClimberId = userId,
                TrainingId = Guid.Parse(trainingId)
            });

            await repo.SaveChangesAsync();
        }

        public async Task LeaveTrainingAsync(string trainingId, string userId)
        {
            TrainingClimber tcToDelete = await repo.GetByIdsAsync<TrainingClimber>(new object[] { userId, Guid.Parse(trainingId) });

            if (tcToDelete != null)
            {
                await repo.DeleteAsync(tcToDelete);
            }
            await repo.SaveChangesAsync();
        }
    }
}
