namespace ClimbingCommunity.Web.Controllers
{
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService _commentService)
        {
            commentService = _commentService;
        }

        [HttpGet]
        public async Task<IActionResult> ActivityComments(string activityId, string activityType)
        {
            ActivityCommentViewModel model = await commentService.GetActivityForCommentById(activityId, activityType);

            model.Comments = await commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId,activityType);

            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment( string activityId,string activityType, ActivityCommentViewModel model)
        { 

            if (string.IsNullOrEmpty(activityId) || string.IsNullOrEmpty(activityType))
            {
               
                return GeneralError();
            }
            if (!ModelState.IsValid)
            {


                model.Comments = await commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId,activityType);

                return View("ActivityComments", model);
            }

            try
            {
                await commentService.AddCommentAsync(activityId, activityType, model.NewCommentText, GetUserId()!);

                return RedirectToAction("ActivityComments", new {  activityId, activityType });
            }
            catch (Exception)
            {

                return GeneralError();
            }
        }


    }
}
