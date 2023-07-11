namespace ClimbingCommunity.Web.ViewModels.Profile
{
    public class CoachProfileViewModel
    {
        public CoachProfileViewModel()
        {
            this.Photos = new HashSet<string>();
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public string Gender { get; set; } = null!; 
        public int CoachingExperience { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public ICollection<string> Photos { get; set; }
    }
}
