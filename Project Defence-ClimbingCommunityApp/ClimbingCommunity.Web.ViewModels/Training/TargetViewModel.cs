namespace ClimbingCommunity.Web.ViewModels.Training
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Target;
    public class TargetViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
