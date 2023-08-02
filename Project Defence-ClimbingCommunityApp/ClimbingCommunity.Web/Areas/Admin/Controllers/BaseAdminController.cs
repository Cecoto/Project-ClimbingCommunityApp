namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using static Common.GeneralApplicationConstants;
    using static Common.RoleConstants;
    using static Common.NotificationMessageConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = Administrator)]
    [AutoValidateAntiforgeryToken]
    public class BaseAdminController : Controller
    {

        protected string? GetUserId()
     => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

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
