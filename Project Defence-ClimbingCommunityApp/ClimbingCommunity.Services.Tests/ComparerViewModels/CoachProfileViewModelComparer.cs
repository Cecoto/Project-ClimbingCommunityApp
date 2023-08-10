namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CoachProfileViewModelComparer : IEqualityComparer<CoachProfileViewModel>
    {
        public bool Equals(CoachProfileViewModel x, CoachProfileViewModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.FirstName == y.FirstName &&
                   x.LastName == y.LastName &&
                   x.PhoneNumber == y.PhoneNumber &&
                   x.ProfilePicture == y.ProfilePicture &&
                   x.Gender == y.Gender &&
                   x.Id == y.Id &&
                   x.CoachingExperience == y.CoachingExperience &&
                   x.TypeOfUser == y.TypeOfUser &&
                   x.Age == y.Age;
        }

        public int GetHashCode(CoachProfileViewModel obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj.FirstName?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.LastName?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.PhoneNumber?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.ProfilePicture?.GetHashCode() ?? 0);
                hash = hash * 23 + obj.Gender.GetHashCode();
                hash = hash * 23 + (obj.Id?.GetHashCode() ?? 0);
                hash = hash * 23 + obj.CoachingExperience.GetHashCode();
                hash = hash * 23 + obj.TypeOfUser.GetHashCode();
                hash = hash * 23 + obj.Age.GetHashCode();
                return hash;
            }
        }
    }

}
