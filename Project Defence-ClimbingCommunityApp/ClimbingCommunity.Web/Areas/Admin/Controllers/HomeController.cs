namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Services;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    /// <summary>
    /// Home controller of the admin area
    /// </summary>
    public class HomeController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;
        /// <summary>
        /// Contructor of the home controller where we inject dependencies.
        /// </summary>
        /// <param name="_userService"></param>
        /// <param name="_roleManager"></param>
        public HomeController(IUserService _userService, RoleManager<IdentityRole> _roleManager)
        {
            userService = _userService;
            roleManager = _roleManager;
        }

        /// <summary>
        /// Get method for reachinh home page of the admin user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            if(!User.IsInRole(RoleConstants.Administrator))
            {
                this.TempData[ErrorMessage] = NoAccessMessage;

                return RedirectToAction("All","ClimbingTrip");
            }
            var viewModel = new UsersRolesViewModel()
            {
                UsersEmails = await userService.GetAllUsersEmailsAsync(),
                RoleNames = roleManager.Roles.Select(r => r.Name).ToList()
            };

            return View(viewModel);
        }
    }
}
