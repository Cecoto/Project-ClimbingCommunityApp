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
            this.JoinedClimbingTrips = new HashSet<JoinedClimbingTripViewModel>();
            this.JoinedTrainings = new HashSet<JoinedTrainingViewModel>();
        }
        public IEnumerable<JoinedClimbingTripViewModel> JoinedClimbingTrips { get; set; }

        public IEnumerable<JoinedTrainingViewModel> JoinedTrainings { get; set; }

    }
}
