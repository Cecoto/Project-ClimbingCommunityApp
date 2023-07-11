namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// A controller where we will manage account of the user.
    /// </summary>
    public class ProfileController : BaseController
    {
        private readonly IUserService userService;
        /// <summary>
        /// Constructor of the controller for injecting needed services.
        /// </summary>
        /// <param name="_userService"></param>
        public ProfileController(IUserService _userService)
        {
            userService = _userService;
        }

        /// <summary>
        /// Method for reaching profile page of the user
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            if (User.IsInRole("Climber"))
            {
                ClimberProfileViewModel model = await userService.GetClimberInfoAsync(GetUserId()!);

                return View("ClimberProfile",model);
            }
            else
            {
                //if (User.IsInRole("Coach"))
                    CoachProfileViewModel model = await userService.GetCoachInfoAsync(GetUserId()!);
                return View("CoachProfile",model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> ManageProfile(string id)
        {


            return View();
        }
    }
}
