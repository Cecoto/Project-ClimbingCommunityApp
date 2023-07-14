namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
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

 
        //protected string? GetUserRole()
        //{
        //    var user = userManager.GetUserAsync(User).Result;

        //    var roles = userManager.GetRolesAsync(user).Result;

        //    return roles.FirstOrDefault() ?? string.Empty;
        //}
        
        

    }
}
