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
        private readonly IImageService imageService;

        /// <summary>
        /// Constructor of the controller for injecting needed services.
        /// </summary>
        /// <param name="_userService"></param>
        /// <param name="_imageService"></param>
        public ProfileController(
            IUserService _userService,
            IImageService _imageService)
        {
            userService = _userService;
            this.imageService = _imageService;

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

        /// <summary>
        /// Get Method for reaching Manage profile page where user can change his profile information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateClimberProfile(string id)
        {

            UpdateClimberProfileViewModel model = await userService.GetClimberInfoForUpdateAsync(id);

            model.ClimberSpecialities = await userService.GetClimberSpecialitiesForFormAsync();

            model.Levels = await userService.GetLevelsForFormAsync();


            return View(model);
        }

       /// <summary>
       /// Post method that updates climbers info.
       /// </summary>
       /// <param name="id"></param>
       /// <param name="model"></param>
       /// <param name="profilePicture"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateClimberProfile(string id,UpdateClimberProfileViewModel model, IFormFile profilePicture)
        {


            if (!ModelState.IsValid)
            {

                model.ClimberSpecialities = await userService.GetClimberSpecialitiesForFormAsync();

                model.Levels = await userService.GetLevelsForFormAsync();

                return View(model);
            }

            if (profilePicture !=null)
            {
                string proofilePictureUrl = await imageService.SaveProfilePictureAsync(profilePicture);

                model.ProfilePictureUrl = proofilePictureUrl;
            }
          

            await userService.UpdateClimberInfoAsync(GetUserId()!,model);

            return RedirectToAction(nameof(MyProfile));

        }
    }
}
