namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Profile;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Common;
    using static Common.NotificationMessageConstants;
    using static Common.GeneralApplicationConstants;

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
            if (!User.Identity?.IsAuthenticated ?? true)
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
            if (user == null)
            {
                this.TempData[ErrorMessage] = "User with that Id does not exist! Please try again later or contact the administrator"!;

                return RedirectToAction("All", "ClimbingTrip");
            }
            if (user.UserType == RoleConstants.Administrator)
            {
                if (!User.IsInRole(RoleConstants.Administrator))
                {
                    this.TempData[ErrorMessage] = "You must be an administrator to have access to that pofile!";
                    return RedirectToAction("All", "ClimbingTrip");
                }
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }
            else if (user.UserType == RoleConstants.Climber)
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
            else /*user.UserType == RoleConstants.Coach*/
            {
                try
                {
                    CoachProfileViewModel model = await userService.GetCoachInfoAsync(id);

                    model.Photos = await userService.GetPhotosForUserAsync(id);

                    if (GetUserId() == id)
                    {
                        model.IsOwner = true;
                    }
                    return View("CoachProfile", model);
                }
                catch (Exception)
                {

                    return GeneralError();
                }

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
            bool isUserExists = await userService.IsUserExistsByIdAsync(id);

            if (!isUserExists)
            {
                this.TempData[ErrorMessage] = "User with that Id does not exist! Please try again later or contact the administrator"!;

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {

                UpdateClimberProfileViewModel model = await userService.GetClimberInfoForUpdateAsync(id);

                model.ClimberSpecialities = await userService.GetClimberSpecialitiesForFormAsync();

                model.Levels = await userService.GetLevelsForFormAsync();


                return View(model);
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }

        /// <summary>
        /// Post method that updates climbers info.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateClimberProfile(UpdateClimberProfileViewModel model)
        {
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
            bool isUserExists = await userService.IsUserExistsByIdAsync(id);

            if (!isUserExists)
            {
                this.TempData[ErrorMessage] = "User with that Id does not exist! Please try again later or contact the administrator"!;

                return RedirectToAction("All", "ClimbingTrip");
            }
            try
            {
                UpdateCoachProfileViewModel model = await userService.GetCoachInfoForUpdateAsync(id);
                return View(model);

            }
            catch (Exception)
            {
                return GeneralError();
            }
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
