namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProfileConroller : BaseController
    {
        private readonly IUserService userService;
        public ProfileConroller(IUserService _userService)
        {
            userService = _userService;
        }

        
        public async Task<IActionResult> MyProfile()
        {
            ClimberProfileViewModel model = await userService.GetClimberInfo(GetUserId()!);

            return View(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> ManageProfile()
        //{
        //    return View();
        //}
    }
}
