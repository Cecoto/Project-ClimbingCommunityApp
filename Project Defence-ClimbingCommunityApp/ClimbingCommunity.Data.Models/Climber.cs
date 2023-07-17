namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    
    /// <summary>
    /// Type of User Climber.
    /// </summary>
    public class Climber : ApplicationUser
    {
        public Climber()
        {
            this.ClimbingTrips = new HashSet<TripClimber>();
            this.JoinedTrainings = new HashSet<TrainingClimber>();
        }

        [ForeignKey(nameof(ClimberSpeciality))]
        public int ClimberSpecialityId { get; set; }
        [Comment("Climber speciality that climber most ofter train and prefer.")]
        public virtual ClimberSpeciality ClimberSpeciality { get; set; } = null!;

        [Comment("Climbing experience that have the climber.")]
        public int ClimbingExperience { get; set; }

        [ForeignKey(nameof(Level))]
        public int LevelId { get; set; }
        [Comment("Personal infomation about the climber on what level in the sport it is.")]
        public virtual Level Level { get; set; } = null!;

        [Comment("Trips that climber is joined to.")]
        public virtual ICollection<TripClimber> ClimbingTrips { get; set; }

        public virtual ICollection<TrainingClimber> JoinedTrainings { get; set; }

    }
}
