namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    /// <summary>
    /// Home controller
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Home ctor with logger
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Show the home page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole("Climber"))
                {
                    return RedirectToAction("LastThreeClimbingTrips", "ClimbingTrip");
                }
                else if (User.IsInRole("Coach"))
                {
                    return RedirectToAction("LastThreeTrainings", "Training");
                }
            }
            return View();
        }

        /// <summary>
        /// Showing page About Us where can find some information about our comminity.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Frequently asked questieons page 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult FAQ()
        {
            return View();
        }

        /// <summary>
        /// Pop up for the user who wants to join our community.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UserTypePopup()
        {
            return PartialView("_UserTypePopupPartial");
        }

        /// <summary>
        /// Default error page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return View("Error404");
            }
           
            return View();
        }
    }
}