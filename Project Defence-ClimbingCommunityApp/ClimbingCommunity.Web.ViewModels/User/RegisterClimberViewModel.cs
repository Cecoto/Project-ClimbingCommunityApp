namespace ClimbingCommunity.Web.ViewModels.User
{
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;
    using static Common.EntityValidationConstants.Climber;


    public class RegisterClimberViewModel
    {
        public RegisterClimberViewModel()
        {
            this.ClimberSpecialities = new HashSet<ClimberSpecialityViewModel>();
            this.Levels = new HashSet<ClimberLevelViewModel>();
        }
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        [Display(Name = "Email address")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ProfilePicture { get; set; }

        [Display(Name = "Profile picture")]
        public IFormFile? PhotoFile { get; set; }

        public string Gender { get; set; } = null!;

        [Range(ClimbingExperienceMinValue, ClimbingExperienceMaxValue)]
        [Display(Name = "Climbing experience (years)")]
        public int ClimbingExperience { get; set; }

        [Display(Name = "Level of climbing")]
        public int LevelId { get; set; }

        [Display(Name = "Speciality")]
        public int ClimberSpecialityId { get; set; }

        public IEnumerable<ClimberLevelViewModel> Levels { get; set; }
        public IEnumerable<ClimberSpecialityViewModel> ClimberSpecialities { get; set; }


    }
}
