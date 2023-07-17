namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
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
        public async Task<IEnumerable<TrainingViewModel>> GetAllJoinedTrainingByUserIdAsync(string userId)
        {
            return await repo.
            //return await repo
            //       .AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
            //       .OrderByDescending(ct => ct.CreatedOn)
            //       .Where(ct => ct.Climbers.Any(c => c.ClimberId == userId))
            //       .Select(ct => new ClimbingTripViewModel()
            //       {
            //           Id = ct.Id.ToString(),
            //           Title = ct.Title,
            //           PhotoUrl = ct.PhotoUrl,
            //           Destination = ct.Destination,
            //           OrganizatorId = ct.OrganizatorId,
            //           Duration = ct.Duration,
            //           TripType = ct.TripType.Name,
            //           isOrganizator = false,
            //           Organizator = ct.Organizator,
            //       }).ToListAsync();
        }

        public Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrainingViewModel>> GetLastThreeTrainingsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
