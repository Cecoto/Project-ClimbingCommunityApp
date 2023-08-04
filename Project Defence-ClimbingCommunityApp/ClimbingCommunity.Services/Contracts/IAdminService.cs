namespace ClimbingCommunity.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task ActivateTrainingByIdAsync(string id);
        Task ActiveteClimbingTripByIdAsync(string id);
        Task BecomeClimberAsync(string adminUserId);
        Task BecomeCoachAsync(string adminUserId);
       
    }
}
