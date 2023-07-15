namespace ClimbingCommunity.Web.ViewModels.ClimbingTrip
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ClimbingTrip;
    public class ClimbingTripFormViewModel
    {
        public ClimbingTripFormViewModel()
        {
            this.TripTypes = new HashSet<TripTypeViewModel>();
            this.IsEditModel = false;
        }
        [Required]
        [StringLength(TitleMaxLength,MinimumLength =TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DestinationMaxLength, MinimumLength = DestinationMinLength)]
        public string Destination { get; set; } = null!;

        public int TripTypeId { get; set; }

        public string? OrganizatorId { get; set; }

        public IEnumerable<TripTypeViewModel> TripTypes { get; set; }

        [Required]
        [Range(DurationMinValue,DurationMaxValue)]
        public int Duration { get; set; }

        [StringLength(PhotoUrlMaxLength)]
        public string? PhotoUrl { get; set; }

        public IFormFile? PhotoFile { get; set; }

        public bool IsEditModel { get; set; }
    }
}
