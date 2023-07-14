namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClimbingTripService
    {
        IEnumerable<ClimbingTripViewModel> GetLastThreeClimbingTrips();

        IEnumerable<ClimbingTripViewModel> GetAllClimbingTrips();
    }
}
