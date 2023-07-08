namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ClimberSpeciality;
    /// <summary>
    /// Climber specality in climbing. 
    /// </summary>
    public class ClimberSpeciality
    {
        [Comment("Entity identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Speciality type")]
        [MaxLength(ClimberSpecialityNameMaxLength)]
        public string Name { get; set; } = null!;

        //[Comment("Property for soft delete")]
        //public bool IsActive { get; set; } 
    }
}
