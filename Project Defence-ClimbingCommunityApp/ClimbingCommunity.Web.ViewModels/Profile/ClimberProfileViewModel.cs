namespace ClimbingCommunity.Web.ViewModels.Profile
{
    using ClimbingCommunity.Web.ViewModels.Comment;

    public class ClimberProfileViewModel
    {
        public ClimberProfileViewModel()
        {
            this.Photos = new HashSet<PhotoViewModel>();
            
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Speciality { get; set; } = null!;
        public string TypeOfUser { get; set; } = null!;
        public int ClimbingExperience { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Level { get; set; } = null!;
        public ICollection<PhotoViewModel> Photos { get; set; }
        public bool IsOwner { get; set; }
        

    }
}
