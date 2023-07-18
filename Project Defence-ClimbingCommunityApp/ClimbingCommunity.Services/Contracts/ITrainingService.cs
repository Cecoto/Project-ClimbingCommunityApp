namespace ClimbingCommunity.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ClimbingCommunity.Web.ViewModels.Training;

    public interface ITrainingService
    {
        Task<IEnumerable<TrainingViewModel>> GetLastThreeTrainingsAsync();
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync();
        Task<IEnumerable<TrainingViewModel>> GetAllJoinedTrainingByUserIdAsync(string userId);

    }
}
