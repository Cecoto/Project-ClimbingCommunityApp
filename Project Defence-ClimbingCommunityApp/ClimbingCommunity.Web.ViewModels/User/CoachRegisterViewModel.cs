namespace ClimbingCommunity.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class CoachRegisterViewModel
    {
        [Required]
        [Phone]
        [StringLength(2)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 10)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(20, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;


    }
}
