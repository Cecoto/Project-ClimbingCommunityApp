namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using static Common.NotificationMessageConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessageConstants.AdminControllerMessages;
    /// <summary>
    /// Admnin controller
    /// </summary>
    public class AdminController : BaseAdminController
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly IClimbingTripService climbingTripService;
        private readonly ITrainingService trainingService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Constructor of the admin controller where we inject needed services in this case.
        /// </summary>
        /// <param name="_adminService"></param>
        /// <param name="_userService"></param>
        /// <param name="_climbingTripService"></param>
        /// <param name="_trainingService"></param>
        /// <param name="_roleManager"></param>
        /// <param name="_userManager"></param>
        public AdminController(
            IAdminService _adminService,
            IUserService _userService,
            IClimbingTripService _climbingTripService,
            ITrainingService _trainingService,
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager)
        {
            adminService = _adminService;
            userService = _userService;
            climbingTripService = _climbingTripService;
            trainingService = _trainingService;
            roleManager = _roleManager;
            userManager = _userManager;

        }
        /// <summary>
        /// Post method for becoming a coach.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BecomeCoach()
        {
            string userId = GetUserId()!;
            bool isUserExist = await userService.IsUserExistsByIdAsync(userId);
            if (!isUserExist)
            {
                this.TempData[ErrorMessage] = InvalidUserIdMessage;

                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole(RoleConstants.Coach))
            {
                this.TempData[WarningMessage] = AlreadyCoachMessage;

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adminService.BecomeCoachAsync(userId);

                this.TempData[SuccessMessage] = BecameCoachMessage;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Post method for becoming a climber.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BecomeClimber()
        {
            string userId = GetUserId()!;
            bool isUserExist = await userService.IsUserExistsByIdAsync(userId);
            if (!isUserExist)
            {
                this.TempData[ErrorMessage] = GeneralErrorMessage;

                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole(RoleConstants.Climber))
            {
                this.TempData[WarningMessage] = AlreadyClimberMessage;

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adminService.BecomeClimberAsync(userId);

                this.TempData[SuccessMessage] = BecameClimberMessage;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        /// <summary>
        /// Get method for showing a page of all users join the application.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Get method for shwoing a page of all activities created by the users.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Post method for activating training.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActivateTraining(string id)
        {
            try
            {
                await adminService.ActivateTrainingByIdAsync(id);

                this.TempData[SuccessMessage] = SuccessfullyActivatedActivityMessage;

                return RedirectToAction("AllActivities", "Admin", new { area = AdminAreaName });
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }
        /// <summary>
        /// Post method for activating climbing trip.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActivateClimbingTrip(string id)
        {
            try
            {
                await adminService.ActivateClimbingTripByIdAsync(id);

                this.TempData[SuccessMessage] = SuccessfullyActivatedActivityMessage;

                return RedirectToAction("AllActivities", "Admin", new { area = AdminAreaName });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        /// <summary>
        /// Methods that creates roles
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost]
        public async Task<IActionResult> CreateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                this.TempData[ErrorMessage] = EmptyRoleMessage;

                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }

            if (await roleManager.RoleExistsAsync(role))
            {
                TempData[ErrorMessage] = RoleAlreadyExistMessage;
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }

            try
            {
                await roleManager.CreateAsync(new IdentityRole(role));

                this.TempData[SuccessMessage] = $"Succesfully created role - {role}!";

                return RedirectToAction("Index", "Home", new { area = AdminAreaName });

            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        /// <summary>
        /// Method that will manualy set to concrete user a role.Will be in the administratorController.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUserToRoles(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                this.TempData[ErrorMessage] = InvalidUserEmailMessage;
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }

            bool isUserInRole = await userManager.IsInRoleAsync(user, role);
            if (isUserInRole)
            {
                this.TempData[ErrorMessage] = AlreadyInRoleMessage;
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }
            try
            {
                await userManager.AddToRoleAsync(user, role);

                this.TempData[SuccessMessage] = $"Successfully added user to role - {role}";

                return RedirectToAction("Index", "Home", new { area = AdminAreaName });

            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
