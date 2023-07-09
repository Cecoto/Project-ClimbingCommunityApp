namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Web.ViewModels;
    using static Common.RoleConstants;
    using ClimbingCommunity.Web.ViewModels.User;

    /// <summary>
    /// Controller about user managment - login, register and logout.
    /// </summary>
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
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
        /// Method for loading the view for register of a climber
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterClimber()
        {
            RegisterClimberViewModel model = new RegisterClimberViewModel();
            return View(model);
        }
    }
}
