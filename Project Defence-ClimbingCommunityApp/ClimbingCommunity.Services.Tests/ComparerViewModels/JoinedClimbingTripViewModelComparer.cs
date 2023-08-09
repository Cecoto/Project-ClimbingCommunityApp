namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class JoinedClimbingTripViewModelComparer : IComparer, IComparer<JoinedClimbingTripViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is JoinedClimbingTripViewModel xModel && y is JoinedClimbingTripViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }

        public int Compare(JoinedClimbingTripViewModel x, JoinedClimbingTripViewModel y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;


            int idComparison = string.Compare(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
            if (idComparison != 0)
                return idComparison;

            int titleComparison = string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase);
            if (titleComparison != 0)
                return titleComparison;

            int photoUrlComparison = string.Compare(x.PhotoUrl, y.PhotoUrl, StringComparison.OrdinalIgnoreCase);
            if (photoUrlComparison != 0)
                return photoUrlComparison;

            int destinationComparison = string.Compare(x.Destination, y.Destination, StringComparison.OrdinalIgnoreCase);
            if (destinationComparison != 0)
                return destinationComparison;

            int organizatorIdComparison = string.Compare(x.OrganizatorId, y.OrganizatorId, StringComparison.OrdinalIgnoreCase);
            if (organizatorIdComparison != 0)
                return organizatorIdComparison;

            int durationComparison = x.Duration.CompareTo(y.Duration);
            if (durationComparison != 0)
                return durationComparison;

            return 0;
        }
    }
}
