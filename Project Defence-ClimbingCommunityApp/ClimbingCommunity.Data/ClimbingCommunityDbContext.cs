namespace ClimbingCommunity.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ClimbingCommunityDbContext : IdentityDbContext
    {
        public ClimbingCommunityDbContext(DbContextOptions<ClimbingCommunityDbContext> options)
            : base(options)
        {
        }
    }
}