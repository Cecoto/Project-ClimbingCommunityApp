namespace ClimbingCommunity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BaseController : Controller
    {
            protected string? GetUserId()
       => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
    }
}
