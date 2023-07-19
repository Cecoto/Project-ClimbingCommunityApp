namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.Comment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task AddCommentAsync(string activityId, string activityType, string newCommentText, string userId);
        Task<ActivityCommentViewModel> GetActivityForCommentById(string activityId, string activityType);
        Task<ICollection<CommentViewModel>> GetAllCommentsByActivityIdAndTypeAsync(string activityId,string activityType);
        Task<bool> IsActivityExistsByIdAndTypeAsync(string activityId, string activityType);
        bool IsActivityTypeExist(string activityType);
    }
}
