namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


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
                isActive = true,
                CreatedOn = DateTime.Now
            };
            if (newCommentText == null)
            {
                return;
            }
            if (activityType == "ClimbingTrip")
            {

                comment.ClimbingTripId = Guid.Parse(activityId);


            }
            else if (activityType == "Training")
            {

                comment.TrainingId = Guid.Parse(activityId);


            }
            else
            {
               
                return;
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
            if (activityType == "ClimbingTrip")
            {

                return await repo.AllReadonly<Comment>(c => c.ClimbingTripId == Guid.Parse(activityId))
                    .OrderByDescending(c=>c.CreatedOn)
                    .Select(c => new CommentViewModel()
                    {
                        Id = c.Id,
                        Text = c.Text,
                        AuthorId = c.AuthorId,
                        Author = c.Author,
                        CreatedOn = c.CreatedOn,
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


        public async Task<bool> IsActivityExistsByIdAndTypeAsync(string activityId, string activityType)
        {
            if (activityType == "ClimbingTrip")
            {
                return await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(activityId)) != null;

            }
            else if (activityType == "Training")
            {
                return await repo.GetByIdAsync<Training>(Guid.Parse(activityId)) != null;

            }
            return false;
        }

        public bool IsActivityTypeExist(string activityType)
        {
            if (activityType == "ClimbingTrip" || activityType == "Training")
            {
                return true;

            }

            return false;
        }
    }
}
