namespace ClimbingCommunity.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required]
        public string EmailAddress { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
