namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using ClimbingCommunity.Common;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    public class HomeController : BaseAdminController
    {
        public IActionResult Index() 
        {
            if(!User.IsInRole(RoleConstants.Administrator))
            {
                this.TempData[ErrorMessage] = "You don't have access to this page!";

                return RedirectToAction("All","ClimbingTrip");
            }
            return View();
        }
    }
}
