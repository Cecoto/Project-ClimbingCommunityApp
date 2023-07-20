namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Profile;

    using static Common.NotificationMessageConstants;
    using ClimbingCommunity.Data.Models;

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
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                TempData["ErrorMessage"] = "You need to be a member of the community to reach that page!";

                return RedirectToAction("Index", "Home");
            }

            string userid = GetUserId()!;
            if (User.IsInRole("Climber"))
            {
                ClimberProfileViewModel model = await userService.GetClimberInfoAsync(userid);

                model.Photos = await userService.GetPhotosForUserAsync(userid);
                model.IsOwner = true;
                return View("ClimberProfile", model);
            }
            else
            {
                //if (User.IsInRole("Coach"))
                CoachProfileViewModel model = await userService.GetCoachInfoAsync(userid);

                model.Photos = await userService.GetPhotosForUserAsync(userid);
                model.IsOwner = true;
                return View("CoachProfile", model);
            }

        }
        /// <summary>
        /// Gets user profile by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserProfile(string id)
        {
            
            ApplicationUser user = await userService.GetUserByIdAsync(id);

            if (user.UserType == "Climber")
            {
                try
                {
                    ClimberProfileViewModel model = await userService.GetClimberInfoAsync(id);

                    model.Photos = await userService.GetPhotosForUserAsync(id);

                    if (GetUserId() == id)
                    {
                        model.IsOwner = true;
                    }

                    return View("ClimberProfile", model);
                }
                catch (Exception)
                {

                    return GeneralError();
                }
                
            }
            else
            {
                CoachProfileViewModel model = await userService.GetCoachInfoAsync(id);

                model.Photos = await userService.GetPhotosForUserAsync(id);

                if (GetUserId() == id)
                {
                    model.IsOwner = true;
                }
                return View("CoachProfile", model);
            }
        }

        /// <summary>
        /// Get Method for reaching Manage climber profile page where user can change his profile information.
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateClimberProfile(UpdateClimberProfileViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.ClimberSpecialities = await userService.GetClimberSpecialitiesForFormAsync();

                model.Levels = await userService.GetLevelsForFormAsync();

                return View(model);
            }

            try
            {
                await userService.UpdateClimberInfoAsync(GetUserId()!, model);

                this.TempData[SuccessMessage] = "Your profile was successfully edited!";

                return RedirectToAction(nameof(MyProfile));

            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Get Method for reaching Manage coach profile page where user can change his profile information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateCoachProfile(string id)
        {

            UpdateCoachProfileViewModel model = await userService.GetCoachInfoForUpdateAsync(id);
            return View(model);
        }
        /// <summary>
        /// Post method that updates climbers info.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateCoachProfile(UpdateCoachProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await userService.UpdateCoachInfoAsync(GetUserId()!, model);

                this.TempData[SuccessMessage] = "Your profile was successfully edited!";

                return RedirectToAction(nameof(MyProfile));
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Method for uploading photos to the users collection.
        /// </summary>
        /// <param name="photos"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadPhotos(List<IFormFile> photos)
        {
            if (photos == null || !photos.Any())
            {
                this.TempData[ErrorMessage] = "No photos selected.";

                return RedirectToAction(nameof(MyProfile));
            }

            List<string> savedPhotoPaths = await imageService.SavePhotosAsync(photos);

            try
            {
                await userService.SavePhotosToUserByIdAsync(GetUserId()!, savedPhotoPaths);

                this.TempData[SuccessMessage] = "Successfully uploaded.";

                return RedirectToAction(nameof(MyProfile));
            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
    }
}
