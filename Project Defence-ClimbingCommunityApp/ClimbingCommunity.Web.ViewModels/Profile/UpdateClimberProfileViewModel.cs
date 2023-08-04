namespace ClimbingCommunity.Web.ViewModels.Profile
{

    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;
    using static Common.EntityValidationConstants.Climber;
    public class UpdateClimberProfileViewModel
    {
        public UpdateClimberProfileViewModel()
        {
            this.ClimberSpecialities = new HashSet<ClimberSpecialityViewModel>();
            this.Levels = new HashSet<ClimberLevelViewModel>();
        }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Range(AgeMinValue,AgeMaxValue)]
        public int Age { get; set; } 

        [Required]
        [Phone]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; } = null!;

        public IFormFile? PhotoFile { get; set; }

        [Range(ClimbingExperienceMinValue, ClimbingExperienceMaxValue)]
        public int ClimbingExperience { get; set; }

        public int LevelId { get; set; }

        public int ClimberSpecialityId { get; set; }

        public IEnumerable<ClimberLevelViewModel> Levels { get; set; }

        public IEnumerable<ClimberSpecialityViewModel> ClimberSpecialities { get; set; }

    }
}
