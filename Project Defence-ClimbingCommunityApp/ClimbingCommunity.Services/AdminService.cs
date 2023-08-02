namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> userManager;
      

        public AdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
           
        }
        public async  Task BecomeClimberAsync(string adminUserId)
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
