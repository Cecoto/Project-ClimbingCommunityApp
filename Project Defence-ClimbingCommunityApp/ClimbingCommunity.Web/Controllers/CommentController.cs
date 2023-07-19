namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Microsoft.AspNetCore.Mvc;

    using static Common.NotificationMessageConstants;
    /// <summary>
    /// Controller for actions related with the comment entity.
    /// </summary>
    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;
        /// <summary>
        /// Constructor for injecting need services.
        /// </summary>
        /// <param name="_commentService"></param>
        public CommentController(ICommentService _commentService)
        {
            commentService = _commentService;
        }

        /// <summary>
        /// Get method for reaching comments section of a concrete activity.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActivityComments(string activityId, string activityType)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be logged in to reach that page!";

                return RedirectToAction("Login", "User");
            }
            bool isActivityTypeExists = commentService.IsActivityTypeExist(activityType);
            if (!isActivityTypeExists)
            {
                this.TempData[ErrorMessage] = "This type of activity does not exist in our climbing community!";
                return RedirectToAction("All", "ClimbingTrip");
            }

            bool isActivityExists = await commentService.IsActivityExistsByIdAndTypeAsync(activityId, activityType);
            if (!isActivityExists)
            {
                this.TempData[ErrorMessage] = "Activity with the provided id does not exist! Please try again.";
                return RedirectToAction("All", "ClimbingTrip");
            }

            try
            {
                ActivityCommentViewModel model = await commentService.GetActivityForCommentById(activityId, activityType);

                model.Comments = await commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId, activityType);

                return View(model);

            }
            catch (Exception)
            {

                return GeneralError();
            }

        }
        /// <summary>
        /// Action for posting new comment to concrete activity.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="activityType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddComment(string activityId, string activityType, ActivityCommentViewModel model)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                this.TempData[ErrorMessage] = "You must be logged in to reach that page!";

                return RedirectToAction("Login", "User");
            }
            bool isActivityTypeExists = commentService.IsActivityTypeExist(activityType);
            if (!isActivityTypeExists)
            {
                this.TempData[ErrorMessage] = "This type of activity does not exist in our climbing community!";
                return RedirectToAction("All", "ClimbingTrip");
            }

            bool isActivityExists = await commentService.IsActivityExistsByIdAndTypeAsync(activityId, activityType);
            if (!isActivityExists)
            {
                this.TempData[ErrorMessage] = "Activity with the provided id does not exist! Please try again.";
                return RedirectToAction("All", "ClimbingTrip");
            }
            if (string.IsNullOrEmpty(activityId) || string.IsNullOrEmpty(activityType))
            {
                return GeneralError();
            }
            if (!ModelState.IsValid)
            {

                model.Comments = await commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId, activityType);

                return View("ActivityComments", model);
            }

            try
            {
                await commentService.AddCommentAsync(activityId, activityType, model.NewCommentText, GetUserId()!);

                this.TempData[SuccessMessage] = "Successfully added a comment!";

                return RedirectToAction("ActivityComments", new { activityId, activityType });
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }


    }
}
