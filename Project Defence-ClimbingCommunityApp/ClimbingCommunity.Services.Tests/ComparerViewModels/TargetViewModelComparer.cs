namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Training;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class TargetViewModelComparer : IComparer, IComparer<TargetViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is TargetViewModel xModel && y is TargetViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }

        public int Compare(TargetViewModel x, TargetViewModel y)
        {
            if (x == null || y == null)
            {
                return -1;
            }

            int idComparison = x.Id.CompareTo(y.Id);

            if (idComparison != 0)
            {
                return idComparison;
            }

            return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
