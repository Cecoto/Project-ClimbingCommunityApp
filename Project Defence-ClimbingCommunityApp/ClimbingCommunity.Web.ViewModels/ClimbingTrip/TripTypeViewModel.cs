namespace ClimbingCommunity.Web.ViewModels.ClimbingTrip
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.TripType;
    public class TripTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength,MinimumLength =NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
