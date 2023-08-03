namespace ClimbingCommunity.Web.ViewModels.AdminArea
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdminAllActivitiesViewModel
    {
        public AdminAllActivitiesViewModel()
        {
            this.ClimbingTrips = new HashSet<AdminActivityViewModel>();
            this.Trainings = new HashSet<AdminActivityViewModel>();
        }
        public IEnumerable<AdminActivityViewModel> ClimbingTrips { get; set; }
        public IEnumerable<AdminActivityViewModel> Trainings { get; set; }
    }
}
