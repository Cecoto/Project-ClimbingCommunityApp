namespace ClimbingCommunity.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Type of User - Coach
    /// </summary>
    public class Coach : ApplicationUser
    {
        public Coach()
        {
            this.Trainings = new HashSet<Training>();
        }
        [Comment("Year of coaching experience that coach have.")]

        public int CoachingExperience { get; set; }

        [Comment("All the training that have created and open to be joined by climbers.")]
        public virtual ICollection<Training> Trainings { get; set; }
    }
}
