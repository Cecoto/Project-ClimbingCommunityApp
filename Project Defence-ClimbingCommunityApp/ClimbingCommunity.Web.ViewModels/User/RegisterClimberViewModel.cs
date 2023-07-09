namespace ClimbingCommunity.Web.ViewModels.User
{
    using ClimbingCommunity.Data.Models;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;
    using static Common.EntityValidationConstants.Climber;


    public class RegisterClimberViewModel
    {
        public RegisterClimberViewModel()
        {
            this.ClimberSpecialities = new HashSet<ClimberSpeciality>();
            this.Levels = new HashSet<Level>();
        }
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
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
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ProfilePicture { get; set; }

        public string Gender { get; set; } = null!;

        [Range(ClimbingExperienceMinValue, ClimbingExperienceMaxValue)]
        public int ClimbingExperience { get; set; }

        public int LevelId { get; set; }

        public int ClimberSpecialityId { get; set; }

        public IEnumerable<Level> Levels { get; set; }
        public IEnumerable<ClimberSpeciality> ClimberSpecialities { get; set; }


    }
}
