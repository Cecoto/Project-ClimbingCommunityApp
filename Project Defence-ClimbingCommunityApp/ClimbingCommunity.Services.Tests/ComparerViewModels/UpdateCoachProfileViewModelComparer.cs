namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UpdateCoachProfileViewModelComparer : IEqualityComparer<UpdateCoachProfileViewModel>
    {
        public bool Equals(UpdateCoachProfileViewModel x, UpdateCoachProfileViewModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.FirstName == y.FirstName &&
                   x.LastName == y.LastName &&
                   x.PhoneNumber == y.PhoneNumber &&
                   x.ProfilePictureUrl == y.ProfilePictureUrl &&
                   x.CoachingExperience == y.CoachingExperience &&
                   x.Age == y.Age;
        }

        public int GetHashCode(UpdateCoachProfileViewModel obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj.FirstName?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.LastName?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.PhoneNumber?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj.ProfilePictureUrl?.GetHashCode() ?? 0);
                hash = hash * 23 + obj.CoachingExperience.GetHashCode();
                hash = hash * 23 + obj.Age.GetHashCode();
                return hash;
            }
        }
    }
}
