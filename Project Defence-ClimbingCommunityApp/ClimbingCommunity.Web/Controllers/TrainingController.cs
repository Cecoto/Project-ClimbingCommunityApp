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
                this.TempData[ErrorMessage] = "You must be a coach to add new training!";

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
                await trainingService.CreateAsync(GetUserId()!,model);

                this.TempData[SuccessMessage] = "Training was added successfully!";

                return RedirectToAction(nameof(LastThreeTrainings));
            }
            catch (Exception)
            {

                return GeneralError();
            }


        }
    }
}
