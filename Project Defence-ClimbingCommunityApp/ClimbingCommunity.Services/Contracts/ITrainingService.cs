namespace ClimbingCommunity.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ClimbingCommunity.Web.ViewModels.Training;

    public interface ITrainingService
    {
        Task<IEnumerable<TrainingViewModel>> GetLastThreeTrainingsAsync();
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync();
        Task<IEnumerable<JoinedTrainingViewModel>> GetAllJoinedTrainingByUserIdAsync(string userId);
        Task<IEnumerable<TargetViewModel>> GetAllTargetsAsync();
        Task<bool> IsUserParticipateInTrainingByIdAsync(string userId, string trainingId);
        Task<bool> IsTargetExistsByIdAsync(int tragetId);
        Task CreateAsync(string organizatorId, TrainingFormViewModel model);
    }
}
