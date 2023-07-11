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
    }
}
