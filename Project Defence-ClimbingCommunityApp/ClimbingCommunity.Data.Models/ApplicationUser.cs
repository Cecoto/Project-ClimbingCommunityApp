namespace ClimbingCommunity.Data.Models
{
    using ClimbingCommunity.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;

    /// <summary>
    /// Base user model that contain common properties of the different type of users.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Photos = new HashSet<Photo>();
            this.Comments = new HashSet<Comment>();
        }
        [Required]
        [Comment("User firstname")]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Comment("User lastname")]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Comment("User age")]
        public int Age { get; set; }

        [Comment("User gender")]
        public Gender Gender { get; set; }

        [Comment("User profile picture")]
        [MaxLength(ImageUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; }

        [Required]
        [Comment("Here we save the user role in the application.")]
        [MaxLength(UserRoleMaxLength)]
        public string UserType { get; set; } = null!;

        [Comment("Collection of photos that user has uploaded to his profile.")]
        public virtual ICollection<Photo> Photos { get; set; }

        [Comment("Collecton of comments that user have commented of posts")]
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
