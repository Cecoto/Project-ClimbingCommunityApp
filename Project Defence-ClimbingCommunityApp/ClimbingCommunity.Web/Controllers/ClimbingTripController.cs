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
            List<ClimbingTripViewModel> models = climbingTripService.GetLastThreeClimbingTrips().ToList();

            string userId = GetUserId()!;

            foreach (ClimbingTripViewModel model in models)
            {
                if (userId == model.OrganizatorId)
                {
                    model.isOrganizator = true;
                }
            }
            return View(models);
        }
        /// <summary>
        /// Method from which we are getting all trips available.
        /// </summary>
        /// <returns>Collection of trips</returns>
        [HttpGet]
        public IActionResult All()
        {
            List<ClimbingTripViewModel> models = climbingTripService.GetAllClimbingTrips().ToList();

            string userId = GetUserId()!;

            foreach (ClimbingTripViewModel model in models)
            {
                if (userId == model.OrganizatorId)
                {
                    model.isOrganizator = true;
                }
            }

            return View(models);
        }
    }
}
