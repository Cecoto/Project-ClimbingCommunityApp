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

        public async Task CreateAsync(string organizatorId, ClimbingTripFormViewModel model)
        {
            if (model.PhotoUrl == null)
            {
                model.PhotoUrl = "/images/ClimbingTrips/Ceuse.jpg";
            }
            ClimbingTrip trip = new ClimbingTrip()
            {
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                Destination = model.Destination,
                OrganizatorId = organizatorId,
                Duration = model.Duration,
                TripTypeId = model.TripTypeId,
            };

            await repo.AddAsync(trip);

            await repo.SaveChangesAsync();
        }

        public async Task EditClimbingTripById(string id, ClimbingTripFormViewModel model)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(id));

            if (model.PhotoUrl==null)
            {
                model.PhotoUrl = "/images/ClimbingTrips/Ceuse.jpg";
            }

            trip.Title = model.Title;
            trip.Duration = model.Duration;
            trip.Destination = model.Destination;
            trip.PhotoUrl = model.PhotoUrl!;
        }

        public async Task<IEnumerable<ClimbingTripViewModel>> GetAllClimbingTripsAsync()
        {
            var models = await repo.AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
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
                 }).ToListAsync();

            return models;
        }

        public async Task<IEnumerable<TripTypeViewModel>> GetAllTripTypesAsync()
        {
            return await repo.AllReadonly<TripType>()
                .Select(ct => new TripTypeViewModel()
                {
                    Id = ct.Id,
                    Name = ct.Name
                }).ToListAsync();

        }

        public async Task<ClimbingTripFormViewModel> GetClimbingTripForEditAsync(string tripId)
        {
            ClimbingTrip trip =  await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId));

            return new ClimbingTripFormViewModel()
            {

                Title = trip.Title,
                PhotoUrl = trip.PhotoUrl,
                Destination = trip.Destination,
                Duration = trip.Duration,
                TripTypeId = trip.TripTypeId,
                OrganizatorId = trip.OrganizatorId,
                IsEditModel = true
            };
                
        }

        public async Task<IEnumerable<ClimbingTripViewModel>> GetLastThreeClimbingTripsAsync()
        {
            var models = await repo.AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
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

                }).ToListAsync();

            return models;

        }

        public async Task<bool> IsClimbingTripExistsByIdAsync(string id)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(id));
            if (trip!= null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsTripTypeExistsByIdAsync(int tripTypeId)
        {
            return await repo.GetByIdAsync<TripType>(tripTypeId) != null;
        }

        public async Task<bool> isUserOrganizatorOfTripById(string userId, string tripId)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId));
            if (trip.OrganizatorId == userId)
            {
                return true;
            }
            return false;
        }
    }
}
