namespace ClimbingCommunity.Web.ViewModels.Training
{
    using ClimbingCommunity.Data.Models;

    public class JoinedTrainingViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Target { get; set; } = null!;

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public string PhotoUrl { get; set; } = null!;

        public string OrganizatorId { get; set; } = null!;

        public Coach Organizator { get; set; } = null!;

        public int NumberOfParticipants { get; set; }
    }
}
