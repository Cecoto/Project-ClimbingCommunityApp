namespace ClimbingCommunity.Web.ViewModels.Training
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Training;
    using static Common.EntityValidationConstants.ApplicationUser;
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class TrainingFormViewModel
    {
        public TrainingFormViewModel()
        {
            this.Targets = new HashSet<TargetViewModel>();
            this.IsEditModel = false;
        }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(LocationMaxLength, MinimumLength = LocationMinLength)]
        public string Location { get; set; } = null!;

        [Range(DurationMinValue, DurationMaxValue)]
        public int Duration { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string? PhotoUrl { get; set; }

        public IFormFile? PhotoFile { get; set; }

        public int TragetId { get; set; }

        public IEnumerable<TargetViewModel> Targets { get; set; }

        [Range(typeof(decimal), PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }

        public string? OrganizatorId { get; set; } 

        public bool IsEditModel { get; set; }

    }
}
