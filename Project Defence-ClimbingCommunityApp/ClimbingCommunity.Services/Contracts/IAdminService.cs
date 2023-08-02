﻿namespace ClimbingCommunity.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task<bool> BecomeClimberAsync(string adminUserId);
        Task<bool> BecomeCoachAsync(string adminUserId);
    }
}
