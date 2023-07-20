namespace ClimbingCommunity.Web.ViewModels.ClimbingTrip
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Web.ViewModels.Comment;

    public class ClimbingTripViewModel
    {
        public ClimbingTripViewModel()
        {
            this.Comments = new HashSet<CommentViewModel>();
        }
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string TripType { get; set; } = null!;

        public string OrganizatorId { get; set; } = null!;

        public Climber Organizator { get; set; } = null!;

        public string PhotoUrl { get; set; } = null!;

        public int Duration { get; set; }

        public bool isOrganizator { get; set; }

        public bool isParticipant { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }


    }
}
