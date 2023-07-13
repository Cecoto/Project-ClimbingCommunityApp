namespace ClimbingCommunity.Web.ViewModels.ClimbingTrip
{
    public class ClimbingTripViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string TripType { get; set; } = null!;

        public string OrganizatorId { get; set; } = null!;

        public bool? isActive { get; set; }

        public string Datetime { get; set; } = null!;

        public string PhotoUrl { get; set; } = null!;

        public int Duration { get; set; }

    }
}
