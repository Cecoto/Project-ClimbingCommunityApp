namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Services;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    public class HomeController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(IUserService _userService, RoleManager<IdentityRole> _roleManager)
        {
            userService = _userService;
            roleManager = _roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            if(!User.IsInRole(RoleConstants.Administrator))
            {
                this.TempData[ErrorMessage] = "You don't have access to this page!";

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
