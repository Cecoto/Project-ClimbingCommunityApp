﻿namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.Training;

    /// <summary>
    /// Training entity
    /// </summary>
    public class Training
    {
        public Training()
        {
            this.Id = Guid.NewGuid();
            this.JoinedClimbers = new HashSet<TrainingClimber>();
            this.Comments = new HashSet<Comment>();
        }

        [Comment("Globaly unique identifier")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Title of the training.")]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [Comment("Location where will be the training - gym, climbing gym etc.")]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        [Comment("Duration of the training in hours.")]
        public int Duration { get; set; }

        [Comment("Price for the training")]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Target))]
        public int TargetId { get; set; }
        [Comment("Information about the target of the training will be - Endurance, Power etc.")]
        public virtual Target Target { get; set; } = null!;

        [ForeignKey(nameof(Coach))]
        public string CoachId { get; set; } = null!;
        [Comment("Give us information about the coach of the training.")]
        public virtual Coach Coach { get; set; } = null!;

        [Comment("Property for soft delete action.")]
        public bool? isActive { get; set; }

        [Comment("Date and time user creted the entity")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Photo of the climbing training will be/Gym picture")]
        public string PhotoUrl { get; set; } = null!;

        [Comment("Collection of joined climbers")]
        public virtual ICollection<TrainingClimber> JoinedClimbers { get; set; }

        [Comment("Collection of all comments posted for this training")]
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
