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
        [Comment("Here we save the userRole in the application.")]
        [MaxLength(UserRoleMaxLength)]
        public string UserType { get; set; } = null!;

    }
}
