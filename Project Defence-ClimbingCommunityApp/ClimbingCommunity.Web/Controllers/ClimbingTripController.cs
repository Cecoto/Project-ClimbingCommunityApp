namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;

    /// <summary>
    /// Controller with all action with climbing trips
    /// </summary>
    public class ClimbingTripController : BaseController
    {
        private readonly IClimbingTripService climbingTripService;
        private readonly ITrainingService trainingService;
        private readonly ICommentService commentService;


        /// <summary>
        /// Constructor of the controller for injecting needed services.
        /// </summary>
        /// <param name="_climbingTripService"></param>
        /// <param name="_trainingService"></param>
        public ClimbingTripController(
            IClimbingTripService _climbingTripService,
            ITrainingService _trainingService,
            ICommentService _commentService)
        {
            this.climbingTripService = _climbingTripService;
            this.trainingService = _trainingService;
            this.commentService = _commentService;
        }


        /// <summary>
        /// Last three climbing trips that climbers posted
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LastThreeClimbingTrips()
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber to have access to that page!";
                return RedirectToAction("LastThreeTrainings", "Training");
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
                    model.isParticipant = await climbingTripService.IsUserParticipateInTripByIdAsync(userId, model.Id);
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
                    else
                    {
                        model.isParticipant = await climbingTripService.IsUserParticipateInTripByIdAsync(userId, model.Id);
                        model.Comments = await commentService.GetAllCommentsByTripId(model.Id);
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
        /// Method that gets all activities that climber has joined.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> JoinedActivites()
        {
            if (!User.IsInRole("Climber"))
            {
                this.TempData["Error Message"] = "You must be a climber to see your joined activities";

                return RedirectToAction(nameof(All));
            }

            try
            {
                string userId = GetUserId()!;

                IEnumerable<JoinedClimbingTripViewModel> joinedTrips = await climbingTripService.GetAllJoinedClimbingTripsByUserIdAsync(userId);

                IEnumerable<JoinedTrainingViewModel> joinedTrainings = await trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

                JoinedActivitiesViewModel model = new JoinedActivitiesViewModel()
                {
                    JoinedClimbingTrips = joinedTrips,
                    JoinedTrainings = joinedTrainings
                };

                return View(model);
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

                this.TempData[SuccessMessage] = "Climbing trip was added successfully!";

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

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);

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

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);

            if (!isOrganizerOfTrip)
            {
                this.TempData[ErrorMessage] = "You must be an organizator of the trip in order to edit climbing trip info!";

                return RedirectToAction("Add", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.EditClimbingTripByIdAsync(id, model);

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
        /// <summary>
        /// Method for soft delete of a climbing trip.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<IActionResult> Delete(string id)
        {
            bool isTripExist = await climbingTripService.IsClimbingTripExistsByIdAsync(id);
            if (!isTripExist)
            {
                this.TempData[ErrorMessage] = "Climbing trip with the provided id does not exist! Please try again.";

                return RedirectToAction("All", "ClimbingTrip");
            }
            if (!User.IsInRole("Climber"))
            {
                this.TempData[ErrorMessage] = "You must be climber in order to delete climbing trips!";
                return RedirectToAction("MyProfile", "Profile");
            }
            bool isClimberOrganizator = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);
            if (!isClimberOrganizator)
            {
                this.TempData[ErrorMessage] = "You must be the the organizator of the trip you want to edit!";

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.DeleteTripByIdAsync(id);

                this.TempData[SuccessMessage] = "Climbing trip was successfully deleted!";

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Method for joining concrete climbing trip 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Join(string id)
        {
            bool isTripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);

            if (!isTripExists)
            {
                this.TempData[ErrorMessage] = "Climbing trip with the provided id does not exist! Please try again.";

                return RedirectToAction("All", "ClimbingTrip");
            }

            if (!User.IsInRole("Climber"))
            {
                this.TempData[ErrorMessage] = "You must be climber in order to join this climbing trips!";
                return RedirectToAction(nameof(All));
            }
            bool isClimberOrganizator = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);
            if (isClimberOrganizator)
            {
                this.TempData[ErrorMessage] = "You are the organizator of the trip!";

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.JoinClimbingTripAsync(id, GetUserId()!);

                this.TempData[SuccessMessage] = "Successfuly joined in that trip!";

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Method for leaving a climbing trip
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Leave(string id)
        {
            bool isTripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);

            if (!isTripExists)
            {
                this.TempData[ErrorMessage] = "Climbing trip with the provided id does not exist! Please try again.";

                return RedirectToAction("All", "ClimbingTrip");
            }
            bool isUserParticipant = await climbingTripService.IsUserParticipateInTripByIdAsync(GetUserId()!, id);
            if (!isUserParticipant)
            {
                this.TempData[ErrorMessage] = "You are not participant of the that trip!";

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.LeaveClimbingTripByIdAsync(id,GetUserId());

                this.TempData[SuccessMessage] = "Successfuly left that trip!";

                return RedirectToAction(nameof(JoinedActivites));
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string id)
        {
            return RedirectToAction();
        }

    }
}
