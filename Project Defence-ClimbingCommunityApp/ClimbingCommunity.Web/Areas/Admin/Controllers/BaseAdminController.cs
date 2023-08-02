namespace ClimbingCommunity.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.GeneralApplicationConstants;
    using static Common.RoleConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = Administrator)]
    [AutoValidateAntiforgeryToken]
    public class BaseAdminController : Controller
    {
    }
}
