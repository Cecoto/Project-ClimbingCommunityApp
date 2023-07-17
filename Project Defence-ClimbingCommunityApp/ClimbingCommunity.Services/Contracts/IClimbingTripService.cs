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
        Task DeleteTripByIdAsync(string tripId);
        Task JoinClimbingTripAsync(string tripId, string userId);
        Task<bool> IsUserParticipateInTripByIdAsync(string userId, string tripId);
        Task LeaveClimbingTripByIdAsync(string tripId,string userId);
    }
}
