namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;

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
        //[Authorize(Roles = "Climber")]
        public async Task<IActionResult> LastThreeClimbingTrips()
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber to add new climbing trips!";
                return RedirectToAction("Index", "Home");
            }
            try
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
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Method from which we are getting all trips available.
        /// </summary>
        /// <returns>Collection of trips</returns>
        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                IEnumerable<ClimbingTripViewModel> models = await climbingTripService.GetAllClimbingTripsAsync();

                string userId = GetUserId()!;
                if (userId == null)
                {
                    this.TempData["Error Message"] = "Invalid ID of a user!";

                    return RedirectToAction("Index", "Home");
                }

                foreach (ClimbingTripViewModel model in models)
                {
                    if (userId == model.OrganizatorId)
                    {
                        model.isOrganizator = true;
                    }
                }

                return View(models);

            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Get method for reaching add page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber to add new climbing trips!";

                return RedirectToAction("Index", "Home");
            }
            try
            {
                ClimbingTripFormViewModel model = new ClimbingTripFormViewModel
                {
                    TripTypes = await climbingTripService.GetAllTripTypesAsync()
                };

                return View(model);

            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Method for created to the database new CLimbing Trip
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(ClimbingTripFormViewModel model)
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber to add new climbing trips!";
                return RedirectToAction("Index", "Home");
            }

            bool tripTypeExists = await climbingTripService.IsTripTypeExistsByIdAsync(model.TripTypeId);
            if (!tripTypeExists)
            {
                ModelState.AddModelError(nameof(model.TripTypeId), "Selected trip type does not exist!");
            }
            if (!ModelState.IsValid)
            {
                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();
                return View(model);
            }

            try
            {

                await climbingTripService.CreateAsync(GetUserId()!, model);

                this.TempData[SuccessMessage] = "House was added successfully!";

                return RedirectToAction(nameof(LastThreeClimbingTrips));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your new Climbing trip! Please try again leter or contact the administartor!");

                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();
                return View(model);
            }
        }
        /// <summary>
        /// Get method for reaching edit from view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber and organize of the trip in order to edit info of the climbing trip!";
                return RedirectToAction("Index", "Home");
            }

            bool tripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);

            if (!tripExists)
            {
                this.TempData[ErrorMessage] = "Climbing trip with the provided id does not exist!";

                return RedirectToAction(nameof(All));
            }

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripById(GetUserId()!, id);

            if (!isOrganizerOfTrip)
            {
                this.TempData[ErrorMessage] = "You must be an organizator of the trip in order to edit climbing trip info!";

                return RedirectToAction("Add", "ClimbingTrip");
            }

            try
            {
                ClimbingTripFormViewModel model = await climbingTripService.GetClimbingTripForEditAsync(id);

                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();

                return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }

        }

        /// <summary>
        /// Method for edit the climbing trip.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ClimbingTripFormViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();
                model.IsEditModel = true;
                return View(model);
            }
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber and organize of the trip in order to edit info of the climbing trip!";
                return RedirectToAction("Index", "Home");
            }

            bool tripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);

            if (!tripExists)
            {
                this.TempData[ErrorMessage] = "Climbing trip with the provided id does not exist!";

                return RedirectToAction(nameof(All));
            }

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripById(GetUserId()!, id);

            if (!isOrganizerOfTrip)
            {
                this.TempData[ErrorMessage] = "You must be an organizator of the trip in order to edit climbing trip info!";

                return RedirectToAction("Add", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.EditClimbingTripById(id, model);

                this.TempData[SuccessMessage] = "Climbing trip was successfully edited!";

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to edit the climbing trip. Please try again later or contact administrator!");

                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();

                return View(model);
            }
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] = "Unexpected error occured! Please try again later or contact administrator.";

            return RedirectToAction("Index", "Home");
        }

    }
}
