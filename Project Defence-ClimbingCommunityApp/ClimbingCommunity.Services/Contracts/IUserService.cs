namespace ClimbingCommunity.Services.Contracts
{
    using ClimbingCommunity.Web.ViewModels.User.Climber;

    public interface IUserService
    {

        Task<IEnumerable<ClimberLevelViewModel>> GetLevelsForRegister();

        Task<IEnumerable<ClimberSpecialityViewModel>> GetClimberSpecialitiesForRegister();

    }
}
