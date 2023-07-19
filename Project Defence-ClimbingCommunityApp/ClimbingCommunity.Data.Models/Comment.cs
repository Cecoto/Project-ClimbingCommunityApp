namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.Comment;
    /// <summary>
    /// Comment entity
    /// </summary>
    public class Comment
    {
       
        [Key]
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(TextMaxLength)]
        [Comment("Content of the comment")]
        public string Text { get; set; } = null!;

        
        public Guid? ClimbingTripId { get; set; }
        public virtual ClimbingTrip? ClimbingTrip { get; set; } 


       
        public Guid? TrainingId { get; set; }
        public virtual Training? Training { get; set; }


        public string AuthorId { get; set; } = null!;
        public virtual ApplicationUser Author { get; set; } = null!;

        public bool? isActive { get; set; }
    }
}
