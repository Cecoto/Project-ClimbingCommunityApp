namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   
    public class AdminActivityViewModelComparer : IComparer, IComparer<AdminActivityViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is AdminActivityViewModel xModel && y is AdminActivityViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }

        public int Compare(AdminActivityViewModel x, AdminActivityViewModel y)
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

            int destinationComparison = string.Compare(x.Location, y.Location, StringComparison.OrdinalIgnoreCase);
            if (destinationComparison != 0)
                return destinationComparison;

            return 0;
        }
    }
}
