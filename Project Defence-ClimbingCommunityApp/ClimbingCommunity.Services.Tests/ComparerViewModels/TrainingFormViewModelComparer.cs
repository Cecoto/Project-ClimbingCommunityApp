namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.Training;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrainingFormViewModelComparer :IEqualityComparer<TrainingFormViewModel>
    {
        public bool Equals(TrainingFormViewModel x, TrainingFormViewModel y)
        {
            if (x == null || y == null)
                return false;

            return x.Title == y.Title &&
                   x.PhotoUrl == y.PhotoUrl &&
                   x.Location == y.Location &&
                   x.Duration == y.Duration &&
                   x.Price == y.Price &&
                   x.OrganizatorId == y.OrganizatorId &&
                   x.TragetId == y.TragetId;
        }

        public int GetHashCode(TrainingFormViewModel obj)
        {
            return HashCode.Combine(obj.Title, obj.PhotoUrl, obj.Location, obj.Duration, obj.Price, obj.OrganizatorId, obj.TragetId);
        }
    }
}
