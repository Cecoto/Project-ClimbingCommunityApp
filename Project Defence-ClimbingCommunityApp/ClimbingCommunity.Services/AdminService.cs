namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository repo;

        public AdminService(
            UserManager<ApplicationUser> _userManager,
            IRepository _repo)
        {
           userManager = _userManager;
           repo = _repo;

        }

        public async Task ActivateTrainingByIdAsync(string id)
        {
           Training trainingForActivation =  await repo.GetByIdAsync<Training>(Guid.Parse(id));

            trainingForActivation.isActive = true;

            await repo.SaveChangesAsync();
        }

        public async Task ActiveteClimbingTripByIdAsync(string id)
        {
            ClimbingTrip trainingForActivation = await repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(id));

            trainingForActivation.IsActive = true;

            await repo.SaveChangesAsync();
        }

        public async Task BecomeClimberAsync(string adminUserId)
        {
            var adminUser = await userManager.FindByIdAsync(adminUserId);

            await userManager.AddToRoleAsync(adminUser, RoleConstants.Climber);

        }

        public async Task BecomeCoachAsync(string adminUserId)
        {
            var adminUser = await userManager.FindByIdAsync(adminUserId);

            await userManager.AddToRoleAsync(adminUser, RoleConstants.Coach);
        }

      
    }
}
