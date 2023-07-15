namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.Profile;
    using ClimbingCommunity.Web.ViewModels.User.Climber;

    public interface IUserService
    {

        Task<IEnumerable<ClimberLevelViewModel>> GetLevelsForFormAsync();

        Task<IEnumerable<ClimberSpecialityViewModel>> GetClimberSpecialitiesForFormAsync();

        Task<ClimberProfileViewModel> GetClimberInfoAsync(string userId);
        Task<CoachProfileViewModel> GetCoachInfoAsync(string userId);

        Task<UpdateClimberProfileViewModel> GetClimberInfoForUpdateAsync(string userId);
        Task UpdateClimberInfoAsync(string userId, UpdateClimberProfileViewModel model);

        Task<UpdateCoachProfileViewModel> GetCoachInfoForUpdateAsync(string id);
        Task UpdateCoachInfoAsync(string userId, UpdateCoachProfileViewModel model);
        Task<bool> IsLevelIdValidByIdAsync(int levelId);
        Task<bool> IsClimbingSpecialityIdValidByIdAsync(int climberSpecialityId);
    }
}
