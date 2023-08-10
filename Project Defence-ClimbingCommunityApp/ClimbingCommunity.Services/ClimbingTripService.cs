namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ClimbingTripService : IClimbingTripService
    {
        private readonly IRepository repo;
        private readonly IImageService imageService;

        public ClimbingTripService(
            IRepository _repo,
            IImageService _imageService
            )
        {
            repo = _repo;
            imageService = _imageService;

        }

        public async Task CreateAsync(string organizatorId, ClimbingTripFormViewModel model)
        {

            if (model.PhotoFile != null)
            {

                model.PhotoUrl = await imageService.SavePictureAsync(model.PhotoFile, "ClimbingTrips");
            }

            ClimbingTrip trip = new ClimbingTrip()
            {
                Title = model.Title,
                PhotoUrl = model.PhotoUrl!,
                Destination = model.Destination,
                OrganizatorId = organizatorId,
                Duration = model.Duration,
                TripTypeId = model.TripTypeId,
            };


            await repo.AddAsync(trip);

            await repo.SaveChangesAsync();
        }

        public async Task DeleteTripByIdAsync(string tripId)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId));

            trip.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task EditClimbingTripByIdAsync(string id, ClimbingTripFormViewModel model)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(id));

            string photo = trip.PhotoUrl;

            if (model.PhotoFile != null)
            {
                if (!string.IsNullOrEmpty(photo))
                {
                    imageService.DeletePicture(photo);
                }

                photo = await imageService.SavePictureAsync(model.PhotoFile, "ClimbingTrips");
            }

            trip.Title = model.Title;
            trip.Duration = model.Duration;
            trip.Destination = model.Destination;
            trip.PhotoUrl = photo;

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClimbingTripViewModel>> GetAllClimbingByStringTripsAsync(string text)
        {
            text = text.ToLower();
            return await repo.AllReadonly<ClimbingTrip>(ct => ct.IsActive == true || ct.IsActive == null)
                 .OrderByDescending(ct => ct.CreatedOn)
                 .Where(ct => ct.Title.ToLower().Contains(text) ||
                 ct.TripType.Name.ToLower().Contains(text) ||
                 ct.Destination.ToLower().Contains(text) ||
                 ct.Organizator.FirstName.ToLower().Contains(text) ||
                 ct.Organizator.LastName.ToLower().Contains(text))
                 .Select(ct => new ClimbingTripViewModel()
                 {
                     Id = ct.Id.ToString(),
                     Title = ct.Title,
                     PhotoUrl = ct.PhotoUrl,
                     Destination = ct.Destination,
                     OrganizatorId = ct.OrganizatorId,
                     Duration = ct.Duration,
                     TripType = ct.TripType.Name,
                     isOrganizator = false,
                     Organizator = ct.Organizator,
                     NumberOfParticipants = ct.Climbers.Count()
                 }).ToListAsync();

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
                     isOrganizator = false,
                     Organizator = ct.Organizator,
                     NumberOfParticipants = ct.Climbers.Count()
                 }).ToListAsync();


            return models;
        }

        public async Task<IEnumerable<JoinedClimbingTripViewModel>> GetAllJoinedClimbingTripsByUserIdAsync(string userId)
        {
            return await repo
                  .AllReadonly<ClimbingTrip>(ct => (ct.IsActive == true || ct.IsActive == null) && ct.Climbers.Any(c => c.ClimberId == userId))
                  .OrderByDescending(ct => ct.CreatedOn)
                  .Select(ct => new JoinedClimbingTripViewModel()
                  {
                      Id = ct.Id.ToString(),
                      Title = ct.Title,
                      PhotoUrl = ct.PhotoUrl,
                      Destination = ct.Destination,
                      OrganizatorId = ct.OrganizatorId,
                      Duration = ct.Duration,
                      TripType = ct.TripType.Name,
                      Organizator = ct.Organizator,
                      NumberOfParticipants = ct.Climbers.Count()
                  }).ToListAsync();

        }

        public async Task<IEnumerable<AdminActivityViewModel>> GetAllTripsForAdminAsync()
        {
            return await repo.AllReadonly<ClimbingTrip>()
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new AdminActivityViewModel()
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = ct.IsActive,
                    Location = ct.Destination
                })
                .ToListAsync();
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
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId));

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
            bool isGuidValid = Guid.TryParse(id,out Guid result);
            if (!isGuidValid)
            {
                return false;
            }
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(id));
            if (trip != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsTripTypeExistsByIdAsync(int tripTypeId)
        {
            return await repo.GetByIdAsync<TripType>(tripTypeId) != null;
        }

        public async Task<bool> isUserOrganizatorOfTripByIdAsync(string userId, string tripId)
        {
            ClimbingTrip trip = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId));
            if (trip.OrganizatorId == userId)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserParticipateInTripByIdAsync(string userId, string tripId)
        {
            return await repo.GetByIdsAsync<TripClimber>(new object[] { Guid.Parse(tripId), userId }) != null;
        }

        public async Task JoinClimbingTripAsync(string tripId, string userId)
        {

            await repo.AddAsync<TripClimber>(new TripClimber()
            {
                ClimberId = userId,
                TripId = Guid.Parse(tripId)
            });

            await repo.SaveChangesAsync();
        }

        public async Task LeaveClimbingTripByIdAsync(string tripId, string userId)
        {
            TripClimber tcToDelete = await repo.GetByIdsAsync<TripClimber>(new object[] { Guid.Parse(tripId), userId });

            if (tcToDelete != null)
            {
                await repo.DeleteAsync<TripClimber>(tcToDelete);
            }

            await repo.SaveChangesAsync();
        }



    }
}
