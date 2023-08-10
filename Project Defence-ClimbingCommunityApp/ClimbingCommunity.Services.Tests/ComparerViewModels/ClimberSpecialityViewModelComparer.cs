namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.Training;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ClimberSpecialityViewModelComparer : IComparer,IComparer<ClimberSpecialityViewModel>
    {

        public int Compare(object x, object y)
        {
            if (x is ClimberSpecialityViewModel xModel && y is ClimberSpecialityViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }

        public int Compare(ClimberSpecialityViewModel x, ClimberSpecialityViewModel y)
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
