namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.AspNetCore.Mvc;
    /// <summary>
    /// Controller for Training entity
    /// </summary>
    public class TrainingController : BaseController
    {
        private readonly ITrainingService trainingService;
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
                this.TempData["Error Message"] = "You must be a coach to have access to that page!";
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
    }
}
