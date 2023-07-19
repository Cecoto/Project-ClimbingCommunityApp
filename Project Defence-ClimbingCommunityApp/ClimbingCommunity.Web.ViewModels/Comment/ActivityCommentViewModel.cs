namespace ClimbingCommunity.Web.ViewModels.Comment
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Comment;

    public class ActivityCommentViewModel
    {
        public ActivityCommentViewModel()
        {
            this.Comments = new HashSet<CommentViewModel>();
        }
        public string? Id { get; set; } 
        public string? Title { get; set; } 
        public string? PhotoUrl { get; set; } 
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public string ActivityType { get; set; } = null!;

        [Required]
        [StringLength(TextMaxLength, MinimumLength = TextMinLength)]
        public string NewCommentText { get; set; } = null!;
    }
}
