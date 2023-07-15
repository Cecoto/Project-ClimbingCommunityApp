namespace ClimbingCommunity.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;


    /// <summary>
    /// Base constroller for global autorizaton of the controllers and implementing common methods.
    /// </summary>
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
        /// <summary>
        /// Method for getting user id.
        /// </summary>
        /// <returns>Logged in user ID</returns>
        protected string? GetUserId()
       => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        /// <summary>
        /// General Error
        /// </summary>
        /// <returns></returns>
        protected IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] = "Unexpected error occured! Please try again later or contact administrator.";

            if (User.IsInRole("Climber"))
            {
                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");

            }
            if (User.IsInRole("Coach"))
            {
                return RedirectToAction("LastThreeTrainings", "Training");

            }
            return RedirectToAction("Index", "Home");
        }



    }
}
