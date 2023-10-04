namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using static Common.GeneralApplicationConstants;
    using static Common.RoleConstants;
    using static Common.NotificationMessageConstants;
    using ClimbingCommunity.Common;
    /// <summary>
    /// Base admin controller only for admin area.
    /// </summary>
    [Area(AdminAreaName)]
    [Authorize(Roles = Administrator)]
    [AutoValidateAntiforgeryToken]
    public class BaseAdminController : Controller
    {
        /// <summary>
        /// Get method for getting current user ID.
        /// </summary>
        /// <returns></returns>
        protected string? GetUserId()
     => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        /// <summary>
        /// General error in admin area
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
            if (User.IsInRole(Administrator))
            {
                return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
