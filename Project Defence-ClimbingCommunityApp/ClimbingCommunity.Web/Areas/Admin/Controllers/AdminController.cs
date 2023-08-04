namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    using static Common.GeneralApplicationConstants;
    public class AdminController : BaseAdminController
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly IClimbingTripService climbingTripService;
        private readonly ITrainingService trainingService;

        public AdminController(
            IAdminService _adminService,
            IUserService _userService,
            IClimbingTripService _climbingTripService,
            ITrainingService _trainingService)
        {
            adminService = _adminService;
            userService = _userService;
            climbingTripService = _climbingTripService;
            trainingService = _trainingService;
        }
        [HttpPost]
        public async Task<IActionResult> BecomeCoach()
        {
            string userId = GetUserId()!;
            bool isUserExist = await userService.IsUserExistsByIdAsync(userId);
            if (!isUserExist)
            {
                this.TempData[ErrorMessage] = "User with provided id does not exist!";

                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole(RoleConstants.Coach))
            {
                this.TempData[WarningMessage] = "You are already a coach!";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adminService.BecomeCoachAsync(userId);

                this.TempData[SuccessMessage] = "You successfully become a coach!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> BecomeClimber()
        {
            string userId = GetUserId()!;
            bool isUserExist = await userService.IsUserExistsByIdAsync(userId);
            if (!isUserExist)
            {
                this.TempData[ErrorMessage] = "User with provided id does not exist!";

                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[WarningMessage] = "You are already a climber!";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adminService.BecomeClimberAsync(userId);

                this.TempData[SuccessMessage] = "You successfully become a climber!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {

            try
            {
                IEnumerable<UserViewModel> models = await userService.GetAllUsersAsync();

                return View(models);
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        [HttpGet]
        public async Task<IActionResult> AllActivities()
        {
            try
            {
                AdminAllActivitiesViewModel allActivities = new AdminAllActivitiesViewModel()
                {
                    ClimbingTrips = await climbingTripService.GetAllTripsForAdminAsync(),
                    Trainings = await trainingService.GetAllTrainingsForAdminAsync()
                };
                return View(allActivities);
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActivateTraining(string id)
        {
            try
            {
                await adminService.ActivateTrainingByIdAsync(id);

                this.TempData[SuccessMessage] = "Succesfully reactivted that activity in the application!";

                return RedirectToAction("AllActivities", "Admin", new { area = AdminAreaName });
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActivateClimbingTrip(string id)
        {
            try
            {
                await adminService.ActiveteClimbingTripByIdAsync(id);

                this.TempData[SuccessMessage] = "Succesfully reactivted that activity in the application!";

                return RedirectToAction("AllActivities", "Admin", new { area = AdminAreaName });
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }


    }
}
