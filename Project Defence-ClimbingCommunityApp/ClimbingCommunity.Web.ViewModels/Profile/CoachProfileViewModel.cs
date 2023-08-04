namespace ClimbingCommunity.Web.ViewModels.Profile
{
    public class CoachProfileViewModel
    {
        public CoachProfileViewModel()
        {
            this.Photos = new HashSet<PhotoViewModel>();
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public string Gender { get; set; } = null!; 
        public int CoachingExperience { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int Age { get; set; }
        public ICollection<PhotoViewModel> Photos { get; set; }
        public string TypeOfUser { get; set; } = null!;
        public bool IsOwner { get; set; }

    }
}
