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

        /// <summary>
        /// Constructor of the userController where we inject needed services for the controler.
        /// </summary>
        /// <param name="_signInManager"></param>
        /// <param name="_roleManager"></param>
        /// <param name="_userService"></param>
        /// <param name="_userManager"></param>
        public UserController(

            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IUserService _userService,
            UserManager<ApplicationUser> _userManager)

        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userService = _userService;
        }
        /// <summary>
        /// Method that will manualy set to concrete user a role.Will be in the administratorController.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> AddUsersToRoles()
        {
            string email1 = "pesho@abv.bg";
            string email2 = "ceco97@asd.bg";

            var pesho = await userManager.FindByEmailAsync(email1);

            var user2 = await userManager.FindByEmailAsync(email2);

            await userManager.AddToRoleAsync(pesho, RoleConstants.Coach);
            await userManager.AddToRoleAsync(user2, RoleConstants.Climber);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Methods that creates roles
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        //TO DO: Only will add this method to administratorController where only admin is allow to manage roles
        public async Task<IActionResult> CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole(Common.RoleConstants.Climber));
            await roleManager.CreateAsync(new IdentityRole(Common.RoleConstants.Coach));
            await roleManager.CreateAsync(new IdentityRole(Administrator));

            return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError(model.Gender, "Invalid gender, please select it again!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
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
                ProfilePictureUrl = "/images/ProfilePictures/male.png",
                UserType = "Coach"

            };


            var result = await userManager.CreateAsync(newCoach, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newCoach, RoleConstants.Coach);

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
                ModelState.AddModelError(model.Gender, "Invalid gender, please select it again!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
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
                ProfilePictureUrl = "/images/ProfilePictures/male.png",
                LevelId = model.LevelId,
                ClimberSpecialityId = model.ClimberSpecialityId,
                UserType = "Climber"

            };


            var result = await userManager.CreateAsync(climber, model.Password);

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(climber, RoleConstants.Climber);

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
                    return RedirectToAction("Index", "Home");
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
