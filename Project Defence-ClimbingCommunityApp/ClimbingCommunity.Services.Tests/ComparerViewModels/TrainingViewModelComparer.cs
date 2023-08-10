namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.Training;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrainingViewModelComparer : IComparer, IComparer<TrainingViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is TrainingViewModel xModel && y is TrainingViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }
        public int Compare(TrainingViewModel x, TrainingViewModel y)
        {
            if (x == null || y == null)
                return 0;

            int idComparison = x.Id.CompareTo(y.Id);
            if (idComparison != 0)
                return idComparison;

            int titleComparison = x.Title.CompareTo(y.Title);
            if (titleComparison != 0)
                return titleComparison;

            int photoUrlComparison = x.PhotoUrl.CompareTo(y.PhotoUrl);
            if (photoUrlComparison != 0)
                return photoUrlComparison;

            int locationComparison = x.Location.CompareTo(y.Location);
            if (locationComparison != 0)
                return locationComparison;

            int durationComparison = x.Duration.CompareTo(y.Duration);
            if (durationComparison != 0)
                return durationComparison;

            int targetComparison = x.Target.CompareTo(y.Target);
            if (targetComparison != 0)
                return targetComparison;

            int priceComparison = x.Price.CompareTo(y.Price);
            if (priceComparison != 0)
                return priceComparison;

            int participantsComparison = x.NumberOfParticipants.CompareTo(y.NumberOfParticipants);
            if (participantsComparison != 0)
                return participantsComparison;

            return 0;
        }
    }
}
