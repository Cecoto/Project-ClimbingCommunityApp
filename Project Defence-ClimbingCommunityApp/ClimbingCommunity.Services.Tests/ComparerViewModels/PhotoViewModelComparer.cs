namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.Profile;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PhotoViewModelComparer : IComparer,IComparer<PhotoViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is PhotoViewModel xModel && y is PhotoViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }

        public int Compare(PhotoViewModel x, PhotoViewModel y)
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

            return string.Compare(x.ImageUrl, y.ImageUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}
