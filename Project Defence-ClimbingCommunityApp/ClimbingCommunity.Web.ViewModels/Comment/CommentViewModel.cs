namespace ClimbingCommunity.Web.ViewModels.Comment
{
    using ClimbingCommunity.Data.Models;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Comment;

    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(TextMaxLength, MinimumLength = TextMinLength)]
        public string Text { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
