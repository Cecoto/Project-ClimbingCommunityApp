namespace ClimbingCommunity.Web.Controllers
{
    using System.Security.Claims;
    using ClimbingCommunity.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using static Common.NotificationMessageConstants;

    using static Common.GeneralApplicationConstants;


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
            this.TempData[ErrorMessage] = GeneralErrorMessage;

            if (User.IsInRole(RoleConstants.Climber))
            {
                return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");

            }
            if (User.IsInRole(RoleConstants.Coach))
            {
                return RedirectToAction("LastThreeTrainings", "Training");

            }
            if (User.IsInRole(RoleConstants.Administrator))
            {
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
