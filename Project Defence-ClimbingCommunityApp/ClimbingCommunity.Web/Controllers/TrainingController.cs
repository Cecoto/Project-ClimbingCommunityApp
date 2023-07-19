namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    /// <summary>
    /// Controller for Training entity
    /// </summary>
    public class TrainingController : BaseController
    {
        private readonly ITrainingService trainingService;
        /// <summary>
        /// Contructor where the services are injected.
        /// </summary>
        /// <param name="_trainingsService"></param>
        public TrainingController(ITrainingService _trainingsService)
        {
            trainingService = _trainingsService;
        }

        /// <summary>
        /// Get method for getting last three trainings poster by coaches/Home page for coaches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LastThreeTrainings()
        {
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to have access to that page!";
                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }

            try
            {
                IEnumerable<TrainingViewModel> models = await trainingService.GetLastThreeTrainingsAsync();
                string userId = GetUserId()!;

                foreach (TrainingViewModel model in models)
                {
                    if (userId == model.OrganizatorId)
                    {
                        model.isOrganizator = true;
                    }
                    model.isParticipant = await trainingService.IsUserParticipateInTrainingByIdAsync(userId, model.Id);
                }
                return View(models);
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Method that get all trainings.Visable for all users of the community.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] string searchString)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be logged in to reach that page!";

                return RedirectToAction("Login", "User");
            }

            try
            {
                IEnumerable<TrainingViewModel> models;
                if (!String.IsNullOrEmpty(searchString))
                {
                    models = await trainingService.GetAllTrainingsByStringAsync(searchString);
                }
                else
                {

                    models = await trainingService.GetAllTrainingsAsync();
                }

                string userId = GetUserId()!;
                foreach (TrainingViewModel model in models)
                {
                    if (model.OrganizatorId == userId)
                    {
                        model.isOrganizator = true;
                    }
                    else
                    {
                        if (User.IsInRole("Climber"))
                        {
                            model.isParticipant = await trainingService.IsUserParticipateInTrainingByIdAsync(userId, model.Id);

                        }
                        else
                        {
                            model.isParticipant = false;
                        }

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
        /// Get method for reaching user trainings.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyTrainings()
        {
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to have trainings!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be a coach to add new training!";

                return RedirectToAction("Login", "User");
            }

            try
            {
                IEnumerable<TrainingViewModel> myTrainings = await trainingService.GetMyTrainingsByIdAsync(GetUserId()!);

                return View(myTrainings);

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
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to add new training!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            try
            {
                TrainingFormViewModel model = new TrainingFormViewModel
                {
                    Targets = await trainingService.GetAllTargetsAsync()
                };

                return View(model);

            }
            catch (Exception)
            {

                return GeneralError();
            }
        }

        /// <summary>
        /// Post method for add functionality.Adding new training.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(TrainingFormViewModel model)
        {
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to add new trainings!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            bool isTargetExists = await trainingService.IsTargetExistsByIdAsync(model.TragetId);
            if (!isTargetExists)
            {
                ModelState.AddModelError(nameof(model.TragetId), "Selected target does not exist! Please try again.");
            }
            if (!ModelState.IsValid)
            {
                model.Targets = await trainingService.GetAllTargetsAsync();
                return View(model);
            }
            try
            {
                await trainingService.CreateAsync(GetUserId()!, model);

                this.TempData[SuccessMessage] = "Training was added successfully!";

                return RedirectToAction(nameof(LastThreeTrainings));
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Get method for reaching edit page of a concrete training.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to edit trainings!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }

            bool isTrainingExists = await trainingService.IsTrainingExistsByIdAsync(id);
            if (!isTrainingExists)
            {
                this.TempData[ErrorMessage] = "Training with the provided id does not exist!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            bool isUserOrganizator = await trainingService.IsUserOrganizatorOfTrainingByIdAsync(GetUserId()!, id);

            if (!isUserOrganizator)
            {
                this.TempData[ErrorMessage] = "You must be an coach of the training in order to edit training info!";

                return RedirectToAction(nameof(Add));
            }
            try
            {
                TrainingFormViewModel model = await trainingService.GetTrainingForEditByIdAsync(id);

                model.Targets = await trainingService.GetAllTargetsAsync();

                return View(model);
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Post method for updating training information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, TrainingFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Targets = await trainingService.GetAllTargetsAsync();
                model.IsEditModel = true;
                return View(model);

            }
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to edit trainings!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }

            bool isTrainingExists = await trainingService.IsTrainingExistsByIdAsync(id);
            if (!isTrainingExists)
            {
                this.TempData[ErrorMessage] = "Training with the provided id does not exist!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            bool isUserOrganizator = await trainingService.IsUserOrganizatorOfTrainingByIdAsync(GetUserId()!, id);

            if (!isUserOrganizator)
            {
                this.TempData[ErrorMessage] = "You must be an coach of the training in order to edit training info!";

                return RedirectToAction(nameof(Add));
            }
            try
            {
                await trainingService.EditTrainingByIdAsync(id, model);

                this.TempData[SuccessMessage] = "Training was successfully edited!";

                return RedirectToAction(nameof(LastThreeTrainings));
            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error occured while trying to edit the climbing trip. Please try again later or contact administrator!";

                model.Targets = await trainingService.GetAllTargetsAsync();
                model.IsEditModel = true;
                return View(model);
            }

        }
        /// <summary>
        /// Post method for solfdelete of a training.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.IsInRole("Coach"))
            {
                this.TempData[ErrorMessage] = "You must be a coach to delete trainings!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }

            bool isTrainingExists = await trainingService.IsTrainingExistsByIdAsync(id);
            if (!isTrainingExists)
            {
                this.TempData[ErrorMessage] = "Training with the provided id does not exist!";

                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
            }
            bool isUserOrganizator = await trainingService.IsUserOrganizatorOfTrainingByIdAsync(GetUserId()!, id);

            if (!isUserOrganizator)
            {
                this.TempData[ErrorMessage] = "You must be an coach of the training in order to delete it!";

                return RedirectToAction(nameof(Add));
            }
            try
            {
                await trainingService.DeleteTrainingByIdAsync(id);

                return RedirectToAction(nameof(LastThreeTrainings));
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Action for joining a training.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Join(string id)
        {
            bool isTrainingExists = await trainingService.IsTrainingExistsByIdAsync(id);

            if (!isTrainingExists)
            {
                this.TempData[ErrorMessage] = "Training with the provided id does not exist! Please try again.";

                return RedirectToAction("All", "Training");
            }
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be logged in to reach that page!";

                return RedirectToAction("Login", "User");
            }
            if (!User.IsInRole("Climber"))
            {
                this.TempData[ErrorMessage] = "You must be climber in order to join this climbing trips!";
                return RedirectToAction(nameof(All));
            }
            bool isUserOrganizator = await trainingService.IsUserOrganizatorOfTrainingByIdAsync(GetUserId()!, id);
            if (isUserOrganizator)
            {
                this.TempData[ErrorMessage] = "You are the organizator of the trip!";

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                await trainingService.JoinTrainingAsync(id, GetUserId()!);

                this.TempData[SuccessMessage] = "Successfuly joined in that training!";

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Action for leaving training.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Leave(string id)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be logged in to reach that page!";

                return RedirectToAction("Login", "User");
            }
            bool isTrainingExists = await trainingService.IsTrainingExistsByIdAsync(id);
            if (!isTrainingExists)
            {
                this.TempData[ErrorMessage] = "Training with the provided id does not exist! Please try again.";

                return RedirectToAction("All", "Training");
            }
            if (!User.IsInRole("Climber"))
            {
                this.TempData[ErrorMessage] = "You must be climber to leave that training!";
                return RedirectToAction(nameof(All));
            }
            bool isUserParticipant = await trainingService.IsUserParticipateInTrainingByIdAsync(GetUserId()!, id);
            if (!isUserParticipant)
            {
                this.TempData[ErrorMessage] = "You are not participant of the that training! You first need to join if you wish to leave it!";

                return RedirectToAction("All", "Training");
            }
            try
            {
                await trainingService.LeaveTrainingAsync(id, GetUserId()!);

                this.TempData[SuccessMessage] = "Successfuly left that training!";

                return RedirectToAction("JoinedActivites", "ClimbingTrip");
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
    }
}
