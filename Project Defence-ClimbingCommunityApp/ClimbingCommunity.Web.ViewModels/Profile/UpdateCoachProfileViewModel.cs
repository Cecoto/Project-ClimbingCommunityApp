namespace ClimbingCommunity.Web.ViewModels.Profile
{
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;
    using static Common.EntityValidationConstants.Coach;

    public class UpdateCoachProfileViewModel
    {

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Range(AgeMinValue, AgeMaxValue)]
        public int Age { get; set; }

        [Required]
        [Phone]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        
        public string PhoneNumber { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; }

        public IFormFile? PhotoFile { get; set; }

        [Range(CoachingExperienceMinValue, CoachingExperienceMaxValue)]
        public int CoachingExperience { get; set; }
    }
}
