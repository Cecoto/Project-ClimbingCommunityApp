namespace ClimbingCommunity.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class TripClimber
    {

        public string ClimberId { get; set; } = null!;
        public virtual Climber Climber { get; set; } = null!;
 
        public Guid TripId { get; set; } 
        public virtual ClimbingTrip ClimbingTrip { get; set; } = null!;
    }
}
