namespace ClimbingCommunity.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task BecomeClimberAsync(string adminUserId);
        Task BecomeCoachAsync(string adminUserId);
       
    }
}
