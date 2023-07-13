namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller with all action with climbing trips
    /// </summary>
    public class ClimbingTripController : BaseController
    {
        private readonly IClimbingTripService climbingTripService;
        /// <summary>
        /// Constructor of the controller for injecting needed services.
        /// </summary>
        /// <param name="_climbingTripService"></param>

        public ClimbingTripController(IClimbingTripService _climbingTripService)
        {
            this.climbingTripService = _climbingTripService;
        }
        /// <summary>
        /// Last three climbing trips that climbers posted
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult LastThreeClimbingTrips()
        {
            IEnumerable<ClimbingTripViewModel> models = climbingTripService.GetLastThreeClimbingTrips();

            return View(models); 
        }
    }
}
