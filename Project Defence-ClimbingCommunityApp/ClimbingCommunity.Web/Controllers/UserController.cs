namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Web.ViewModels;
    using ClimbingCommunity.Web.ViewModels.User;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Data.Models.Enums;

    using static Common.RoleConstants;
    using static Common.NotificationMessageConstants;
    using ClimbingCommunity.Common;

    /// <summary>
    /// Controller about user managment - login, register and logout.
    /// </summary>
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;
        private readonly IImageService imageService;

        /// <summary>
        /// Constructor of the userController where we inject needed services for the controler.
        /// </summary>
        /// <param name="_signInManager"></param>
        /// <param name="_roleManager"></param>
        /// <param name="_userService"></param>
        /// <param name="_userManager"></param>
        /// <param name="_imageService"></param>
        public UserController(
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IUserService _userService,
            UserManager<ApplicationUser> _userManager,
            IImageService _imageService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userService = _userService;
            imageService = _imageService;
        }
        /// <summary>
        /// Method for loading the form for register of a coach.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterCoach()
        {
            RegisterCoachViewModel model = new RegisterCoachViewModel();
            return View(model);
        }
        /// <summary>
        /// Post method for registration of the Coach and setting him a role.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Redirection to login page</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCoach(RegisterCoachViewModel model)
        {
            bool genderIsValid = Enum.TryParse<Gender>(model.Gender, out Gender gender);

            if (!genderIsValid)
            {
                ModelState.AddModelError(nameof(model.Gender), "Invalid gender, please select it again!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PhotoFile != null)
            {
                model.ProfilePicture = await imageService.SavePictureAsync(model.PhotoFile, "ProfilePictures");
            }

            if (String.IsNullOrWhiteSpace(model.ProfilePicture))
            {
                if (model.Gender == "Male")
                {
                    model.ProfilePicture = "/images/ProfilePictures/male.png";
                }
                else
                {
                    model.ProfilePicture = "/images/ProfilePictures/Female.png";
                }

            }


            Coach newCoach = new Coach()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CoachingExperience = model.CoachingExperience,
                Gender = gender,
                ProfilePictureUrl = model.ProfilePicture,
                UserType = "Coach",
                Age = model.Age

            };


            var result = await userManager.CreateAsync(newCoach, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newCoach, RoleConstants.Coach);

                this.TempData[SuccessMessage] = "You successfully joined our community !";

                return RedirectToAction("Login", "User");
            }

            foreach (var error in result.Errors)
            {

                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }
        /// <summary>
        /// Method for loading the view for register of a climber
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClimber()
        {
            RegisterClimberViewModel model = new RegisterClimberViewModel()
            {
                Levels = await userService.GetLevelsForFormAsync(),
                ClimberSpecialities = await userService.GetClimberSpecialitiesForFormAsync()

            };
            return View(model);
        }
        /// <summary>
        /// Post method for registration of the Climber and setting him a role.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Redirection to login page</returns>

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClimber(RegisterClimberViewModel model)
        {

            bool genderIsValid = Enum.TryParse<Gender>(model.Gender, out Gender gender);

            if (!genderIsValid)
            {
                ModelState.AddModelError(nameof(model.Gender), "Invalid gender, please select it again!");
            }

            bool isLevelValid = await userService.IsLevelIdValidByIdAsync(model.LevelId);

            if (!isLevelValid)
            {
                ModelState.AddModelError(nameof(model.LevelId), "Invalid level id, please select it again!");
            }

            bool isClimbingSpecialityValid = await userService.IsClimbingSpecialityIdValidByIdAsync(model.ClimberSpecialityId);

            if (!isClimbingSpecialityValid)
            {
                ModelState.AddModelError(nameof(model.ClimberSpecialityId), "Invalid speciality id, please select it again!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PhotoFile != null)
            {
                model.ProfilePicture = await imageService.SavePictureAsync(model.PhotoFile, "ProfilePictures");
            }

            if (String.IsNullOrWhiteSpace(model.ProfilePicture))
            {
                if (model.Gender == "Male")
                {
                    model.ProfilePicture = "/images/ProfilePictures/male.png";
                }
                else
                {
                    model.ProfilePicture = "/images/ProfilePictures/Female.png";
                }

            }

            Climber climber = new Climber()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ClimbingExperience = model.ClimbingExperience,
                Gender = gender,
                ProfilePictureUrl = model.ProfilePicture,
                LevelId = model.LevelId,
                ClimberSpecialityId = model.ClimberSpecialityId,
                UserType = "Climber",
                Age = model.Age

            };


            var result = await userManager.CreateAsync(climber, model.Password);

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(climber, RoleConstants.Climber);

                this.TempData[SuccessMessage] = "You successfully joined our community!";

                return RedirectToAction("Login", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        /// <summary>
        /// Get method for loading the login page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        /// <summary>
        /// Post method for login of the user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = await userManager.FindByEmailAsync(model.EmailAddress);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {

                    if (user.UserType == RoleConstants.Climber)
                    {
                        return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");

                    }
                    if (user.UserType == RoleConstants.Coach)
                    {
                        return RedirectToAction("LastThreeTrainings", "Training");

                    }
                    if (user.UserType == Administrator)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                   
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login! Please try again.");

            return View(model);
        }

        /// <summary>
        /// Method for logout of the user.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
