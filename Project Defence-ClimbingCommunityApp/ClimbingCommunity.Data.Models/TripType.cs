namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.TripType;

    /// <summary>
    /// Climbing trip type entity
    /// </summary>
    public class TripType
    {
        [Comment("Trip identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Trip name/title")]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

    }
}
