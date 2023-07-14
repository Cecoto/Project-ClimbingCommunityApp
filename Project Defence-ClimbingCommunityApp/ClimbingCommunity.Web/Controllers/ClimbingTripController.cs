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
        public async Task<IActionResult> LastThreeClimbingTrips()
        {
            IEnumerable<ClimbingTripViewModel> models = await climbingTripService.GetLastThreeClimbingTripsAsync();

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
        public async Task<IActionResult> All()
        {
            IEnumerable<ClimbingTripViewModel> models = await climbingTripService.GetAllClimbingTripsAsync();

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
        /// Get method for reaching add page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ClimbingTripFormViewModel model = new ClimbingTripFormViewModel
            {
                TripTypes = await climbingTripService.GetAllClimbingTripTypesAsync()
            };

            return View(model);
        }
        /// <summary>
        /// Method for created to the database new CLimbing Trip
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult>Add(ClimbingTripFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TripTypes = await climbingTripService.GetAllClimbingTripTypesAsync();
                return View(model);
            }

            await climbingTripService.CreateAsync(GetUserId()!,model);

            return RedirectToAction(nameof(LastThreeClimbingTrips));
        }
    }
}
