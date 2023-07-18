namespace ClimbingCommunity.Data
{
    using ClimbingCommunity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using System.Reflection;

    public class ClimbingCommunityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ClimbingCommunityDbContext(DbContextOptions<ClimbingCommunityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Climber> Climbers { get; set; } = null!;

        public DbSet<ClimberSpeciality> ClimberSpecialities { get; set; } = null!;

        public DbSet<ClimbingTrip> ClimbingTrips { get; set; } = null!;

        public DbSet<Coach> Coaches { get; set; } = null!;

        public DbSet<Level> Levels { get; set; } = null!;

        public DbSet<Training> Trainings { get; set; } = null!;

        public DbSet<TripType> TripTypes { get; set; } = null!;

        public DbSet<TripClimber> TripsClimbers { get; set; } = null!;

        public DbSet<Photo> Photos { get; set; } = null!;

        public DbSet<TrainingClimber> TrainingsClimbers { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(ClimbingCommunityDbContext)) ?? Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);

        }
    }
}