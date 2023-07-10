namespace ClimbingCommunity.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using WebShopDemo.Core.Data.Common;

    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<ClimberSpecialityViewModel>> GetClimberSpecialitiesForRegister()
        {
            return await repo.AllReadonly<ClimberSpeciality>()
                .Select(cs=> new ClimberSpecialityViewModel()
                {
                    Id = cs.Id,
                    Name = cs.Name,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ClimberLevelViewModel>> GetLevelsForRegister()
        {
            return await repo.AllReadonly<Level>()
                .Select(cs => new ClimberLevelViewModel()
                {
                    Id = cs.Id,
                    Name = cs.Name,
                })
                .ToListAsync();
        }
    }
}