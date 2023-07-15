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
            //TO DO: if user is logged in to redirect him to different page!!!
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}