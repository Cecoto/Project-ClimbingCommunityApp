namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClimbingTripService
    {
        Task<IEnumerable<ClimbingTripViewModel>> GetLastThreeClimbingTripsAsync();

        Task<IEnumerable<ClimbingTripViewModel>> GetAllClimbingTripsAsync();

        Task<IEnumerable<TripTypeViewModel>> GetAllClimbingTripTypesAsync();
        Task CreateAsync(string organizatorId, ClimbingTripFormViewModel model);
    }
}
