namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    public class TrainingService : ITrainingService
    {
        private readonly IRepository repo;
        public TrainingService(IRepository _repo)
        {
            repo = _repo;
        }
        public async Task<IEnumerable<JoinedTrainingViewModel>> GetAllJoinedTrainingByUserIdAsync(string userId)
        {
            return await repo
                     .AllReadonly<Training>(t => (t.isActive == true || t.isActive == null) && t.JoinedClimbers.Any(c=>c.ClimberId==userId))
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

        public Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync()
        {
            throw new NotImplementedException();
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

        public async Task<bool> IsUserParticipateInTrainingByIdAsync(string userId, string trainingId)
        {
            return await repo.GetByIdsAsync<TrainingClimber>(new object[] { userId, Guid.Parse(trainingId)}) != null;
        }
    }
}
