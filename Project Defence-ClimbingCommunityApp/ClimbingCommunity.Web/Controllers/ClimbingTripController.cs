﻿namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using ClimbingCommunity.Common; 

    using static Common.NotificationMessageConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessageConstants.ClimbingTripControllerMessages;


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
        /// <param name="_commentService"></param>
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
            if (!User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[ErrorMessage] = MustBeClimberMessage;

                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
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
                    if (User.IsInRole(RoleConstants.Administrator))
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
        public async Task<IActionResult> All([FromQuery] string searchString)
        {

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = MustBeLoggedInMessage;

                return RedirectToAction("Login", "User");
            }
            try
            {
                IEnumerable<ClimbingTripViewModel> models;
                if (!String.IsNullOrEmpty(searchString))
                {
                    models = await climbingTripService.GetAllClimbingByStringTripsAsync(searchString);
                }
                else
                {
                    models = await climbingTripService.GetAllClimbingTripsAsync();

                }
                string userId = GetUserId()!;

                foreach (ClimbingTripViewModel model in models)
                {
                    if (userId == model.OrganizatorId)
                    {
                        model.isOrganizator = true;
                    }

                    if (User.IsInRole(RoleConstants.Administrator))
                    {
                        model.isOrganizator = true;
                    }

                    else
                    {
                        model.isParticipant = await climbingTripService.IsUserParticipateInTripByIdAsync(userId, model.Id);
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
            if (!User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[ErrorMessage] = MustBeClimberMessage;

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
            if (!User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[ErrorMessage] = MustBeClimberToAddMessage;

                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("LastThreeTrainings", "Training");
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
                this.TempData[ErrorMessage] = MustBeClimberToAddMessage;

                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("LastThreeTrainings", "Training");
            }

            bool tripTypeExists = await climbingTripService.IsTripTypeExistsByIdAsync(model.TripTypeId);

            if (!tripTypeExists)
            {
                ModelState.AddModelError(nameof(model.TripTypeId), InvalidTripTypeMessage);
            }
            if (!ModelState.IsValid)
            {
                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();
                return View(model);
            }

            try
            {

                await climbingTripService.CreateAsync(GetUserId()!, model);

                this.TempData[SuccessMessage] = SuccessfullyAddedMessage;

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

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
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;
                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }
            bool tripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);

            if (!tripExists)
            {
                this.TempData[ErrorMessage] = InvalidClimbingTripIdMessage;

                return RedirectToAction(nameof(All));
            }

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);

            if (User.IsInRole(RoleConstants.Administrator))
            {
                isOrganizerOfTrip = true;
            }
            if (!isOrganizerOfTrip)
            {
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;

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
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;

                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }

            bool tripExists = await climbingTripService.IsClimbingTripExistsByIdAsync(id);
            if (!tripExists)
            {
                this.TempData[ErrorMessage] = InvalidClimbingTripIdMessage;

                return RedirectToAction(nameof(All));
            }

            bool isOrganizerOfTrip = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);

            if (User.IsInRole(RoleConstants.Administrator))
            {
                isOrganizerOfTrip = true;
            }
            if (!isOrganizerOfTrip)
            {
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;

                return RedirectToAction("Add", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.EditClimbingTripByIdAsync(id, model);

                this.TempData[SuccessMessage] = SuccessfullyEditedMessage;

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

                model.TripTypes = await climbingTripService.GetAllTripTypesAsync();

                return View(model);
            }
        }
        /// <summary>
        /// Method for soft delete of a climbing trip.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            bool isTripExist = await climbingTripService.IsClimbingTripExistsByIdAsync(id);
            if (!isTripExist)
            {
                this.TempData[ErrorMessage] = InvalidClimbingTripIdMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }
            if (!User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;
                if (User.IsInRole(RoleConstants.Administrator))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("MyProfile", "Profile");
            }
            bool isClimberOrganizator = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);
            if (User.IsInRole(RoleConstants.Administrator))
            {
                isClimberOrganizator = true;
            }
            if (!isClimberOrganizator)
            {
                this.TempData[ErrorMessage] = MustBeOrganizatorMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.DeleteTripByIdAsync(id);

                if (User.IsInRole(RoleConstants.Administrator))
                {
                    this.TempData[SuccessMessage] = SuccessfullyDeletedMessage;

                    return RedirectToAction("AllActivities", "Admin", new { area = AdminAreaName });
                }
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
                this.TempData[ErrorMessage] = InvalidClimbingTripIdMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }

            if (!User.IsInRole("Climber"))
            {
                this.TempData[ErrorMessage] = MustBeClimberToJoinTripMessage;
                return RedirectToAction(nameof(All));
            }
            bool isClimberOrganizator = await climbingTripService.isUserOrganizatorOfTripByIdAsync(GetUserId()!, id);
            if (isClimberOrganizator)
            {
                this.TempData[ErrorMessage] = YouAreOrganizatorMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.JoinClimbingTripAsync(id, GetUserId()!);

                this.TempData[SuccessMessage] = SuccessfullyJoinedMessage;

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
                this.TempData[ErrorMessage] = InvalidClimbingTripIdMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }
            bool isUserParticipant = await climbingTripService.IsUserParticipateInTripByIdAsync(GetUserId()!, id);
            if (!isUserParticipant)
            {
                this.TempData[ErrorMessage] = NotParticipantMessage;

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await climbingTripService.LeaveClimbingTripByIdAsync(id, GetUserId()!);

                this.TempData[SuccessMessage] = SuccessfullyLeftMessage;

                return RedirectToAction(nameof(JoinedActivites));
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }

    }
}
