namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Level;

    /// <summary>
    /// Entity for level of the climber.
    /// </summary>
    public class Level
    {
        [Comment("Entity identifier")]
        public int Id { get; set; }
        [Required]
        [Comment("Name/Title of the level.")]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        //[Comment("Property for soft delete")]
        //public bool IsActive { get; set; }
    }
}
