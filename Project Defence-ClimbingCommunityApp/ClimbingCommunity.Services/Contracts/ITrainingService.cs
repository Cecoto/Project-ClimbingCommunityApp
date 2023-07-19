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
        Task<bool> IsTrainingExistsByIdAsync(string trainingId);
        Task<bool> IsUserOrganizatorOfTrainingByIdAsync(string userId, string trainingId);
        Task<TrainingFormViewModel> GetTrainingForEditByIdAsync(string trainingId);
        Task EditTrainingByIdAsync(string trainingId, TrainingFormViewModel model);
        Task DeleteTrainingByIdAsync(string trainingId);
        Task<IEnumerable<TrainingViewModel>> GetMyTrainingsByIdAsync(string userId);
        Task JoinTrainingAsync(string trainingId, string userId);
        Task LeaveTrainingAsync(string trainingId, string userId);
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingsByStringAsync(string searchString);
    }
}
