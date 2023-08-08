namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ClimbingTripViewModelComparer : IComparer<ClimbingTripViewModel>
    {
        public int Compare(ClimbingTripViewModel x, ClimbingTripViewModel y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            int idComparison = x.Id.CompareTo(y.Id);
            if (idComparison != 0)
                return idComparison;

            int titleComparison = x.Title.CompareTo(y.Title);
            if (titleComparison != 0)
                return titleComparison;

            int photoUrlComparison = x.PhotoUrl.CompareTo(y.PhotoUrl);
            if (photoUrlComparison != 0)
                return photoUrlComparison;

            int destinationComparison = x.Destination.CompareTo(y.Destination);
            if (destinationComparison != 0)
                return destinationComparison;

            int organizatorIdComparison = x.OrganizatorId.CompareTo(y.OrganizatorId);
            if (organizatorIdComparison != 0)
                return organizatorIdComparison;

            int durationComparison = x.Duration.CompareTo(y.Duration);
            if (durationComparison != 0)
                return durationComparison;

            int tripTypeComparison = x.TripType.CompareTo(y.TripType);
            if (tripTypeComparison != 0)
                return tripTypeComparison;

            int isOrganizatorComparison = x.isOrganizator.CompareTo(y.isOrganizator);
            if (isOrganizatorComparison != 0)
                return isOrganizatorComparison;

            return 0;
        }
    }

}
