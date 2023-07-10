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
    /// <summary>
    /// Controller about user managment - login, register and logout.
    /// </summary>
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public UserController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IUserService _userService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userService = _userService;
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
        /// Post method for registration of the coach.
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
                ProfilePictureUrl = "/images/ProfilePictures/male.png"                
            };

            //await userManager.AddToRoleAsync(newCoach,Common.RoleConstants.Coach);
            
            var result = await userManager.CreateAsync(newCoach,model.Password);
            if (result.Succeeded)
            {
                return Ok();
                //return RedirectToAction("Login", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);
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
                Levels = await userService.GetLevelsForRegister(),
                ClimberSpecialities = await userService.GetClimberSpecialitiesForRegister()

            };
            return View(model);
        }
        /// <summary>
        /// Post method for registration of the climber
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
                ClimberSpecialityId = model.ClimberSpecialityId
            };

            //await userManager.AddToRoleAsync(newCoach,Common.RoleConstants.Coach);

            var result = await userManager.CreateAsync(climber, model.Password);
            if (result.Succeeded)
            {
                return Ok();
                //return RedirectToAction("Login", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
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
            ModelState.AddModelError(string.Empty,"Invalid login! Please try again.");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
