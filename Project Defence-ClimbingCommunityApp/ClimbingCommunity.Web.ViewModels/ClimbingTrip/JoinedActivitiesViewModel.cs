namespace ClimbingCommunity.Web.ViewModels.ClimbingTrip
{
    using ClimbingCommunity.Web.ViewModels.Training;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class JoinedActivitiesViewModel
    {
        public JoinedActivitiesViewModel()
        {
            this.JoinedClimbingTrips = new HashSet<ClimbingTripViewModel>();
            this.JoinedTrainings = new HashSet<TrainingViewModel>();
        }
        public IEnumerable<ClimbingTripViewModel> JoinedClimbingTrips { get; set; }

        public IEnumerable<TrainingViewModel> JoinedTrainings { get; set; }

    }
}
