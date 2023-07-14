namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClimbingTripService
    {
        Task<IEnumerable<ClimbingTripViewModel>> GetLastThreeClimbingTripsAsync();

        Task<IEnumerable<ClimbingTripViewModel>> GetAllClimbingTripsAsync();

        Task<IEnumerable<TripTypeViewModel>> GetAllTripTypesAsync();
        Task CreateAsync(string organizatorId, ClimbingTripFormViewModel model);
        Task<bool> IsTripTypeExistsByIdAsync(int tripTypeId);
        Task<bool> isUserOrganizatorOfTripById(string userId, string tripId);
        Task<bool> IsClimbingTripExistsByIdAsync(string id);
        Task<ClimbingTripFormViewModel> GetClimbingTripForEditAsync(string tripId);
        Task EditClimbingTripById(string id, ClimbingTripFormViewModel model);
    }
}
