namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    public class ClimbingTripService : IClimbingTripService
    {
        private readonly IRepository repo;

        public ClimbingTripService(IRepository _repo)
        {
            repo = _repo;
        }

        public IEnumerable<ClimbingTripViewModel> GetAllClimbingTrips()
        {
            var models = repo.AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
                 .OrderByDescending(ct => ct.CreatedOn)  
                 .Select(ct => new ClimbingTripViewModel()
                 {
                     Id = ct.Id.ToString(),
                     Title = ct.Title,
                     PhotoUrl = ct.PhotoUrl,
                     Destination = ct.Destination,
                     OrganizatorId = ct.OrganizatorId,
                     Duration = ct.Duration,
                     TripType = ct.TripType.Name,
                     isOrganizator = false

                 });

            return models;
        }

        public IEnumerable<ClimbingTripViewModel> GetLastThreeClimbingTrips()
        {
            var models =  repo.AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct=>ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel()
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                    
                });

            return models;
            
        }
    }
}
