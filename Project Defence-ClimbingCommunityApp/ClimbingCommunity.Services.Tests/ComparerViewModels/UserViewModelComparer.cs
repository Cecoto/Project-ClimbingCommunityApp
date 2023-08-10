namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserViewModelComparer : IComparer, IComparer<UserViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is UserViewModel xModel && y is UserViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }
        public int Compare(UserViewModel x, UserViewModel y)
        {
            if (x == null || y == null)
            {
                return -1;
            }

            if (x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.Email == y.Email &&
                x.Age == y.Age &&
                x.Role == y.Role)
            {
                return 0;
            }

            return -1;
        }
    }

}
