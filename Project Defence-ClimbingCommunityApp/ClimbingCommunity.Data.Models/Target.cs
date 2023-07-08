namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Target;
    /// <summary>
    /// Target entity for showing the type of the training.
    /// </summary>
    public class Target
    {
        [Comment("Entity identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Show us the target of the training")]
        public string Name { get; set; } = null!;

        //[Comment("Property for soft delete")]
        //public bool IsActive { get; set; } 
    }
}
