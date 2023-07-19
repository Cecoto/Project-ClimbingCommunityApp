namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;
    using static System.Net.Mime.MediaTypeNames;

    public class CommentService : ICommentService
    {
        private readonly IRepository repo;
        public CommentService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddCommentAsync(string activityId, string activityType, string newCommentText, string userId)
        {
            Comment comment = new Comment
            {
                Text = newCommentText,
                AuthorId = userId,
                Author = await repo.GetByIdAsync<ApplicationUser>(userId),
                isActive = true
            };
            if (activityType == "ClimbingTrip")
            {

                comment.ClimbingTripId = Guid.Parse(activityId);
                

            }
            else if (activityType == "Training")
            {

                comment.TrainingId = Guid.Parse(activityId);
                

            }

            await repo.AddAsync<Comment>(comment);

            await repo.SaveChangesAsync();
        }

        public async Task<ActivityCommentViewModel> GetActivityForCommentById(string activityId, string activityType)
        {
            ActivityCommentViewModel model = new ActivityCommentViewModel()
            {
                Id = activityId
            };

            if (activityType == "ClimbingTrip")
            {
                ClimbingTrip activity = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(activityId));

                model.Title = activity.Title;
                model.PhotoUrl = activity.PhotoUrl;
                model.ActivityType = activityType;

            }
            else if (activityType == "Training")
            {
                Training activity = await repo.GetByIdAsync<Training>(Guid.Parse(activityId));

                model.Title = activity.Title;
                model.PhotoUrl = activity.PhotoUrl;
                model.ActivityType = activityType;
            }
            return model;
        }

        public async Task<ICollection<CommentViewModel>> GetAllCommentsByActivityIdAndTypeAsync(string activityId, string activityType)
        {
            if (activityType=="ClimbingTrip")
            {

            return await repo.AllReadonly<Comment>(c => c.ClimbingTripId == Guid.Parse(activityId))
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    AuthorId = c.AuthorId,
                    Author = c.Author
                })
                .ToListAsync();
            }
            else
            {
                return await repo.AllReadonly<Comment>(c => c.TrainingId == Guid.Parse(activityId))
               .Select(c => new CommentViewModel()
               {
                   Id = c.Id,
                   Text = c.Text,
                   AuthorId = c.AuthorId,
                   Author = c.Author
               })
               .ToListAsync();
            }
        }
    }
}
