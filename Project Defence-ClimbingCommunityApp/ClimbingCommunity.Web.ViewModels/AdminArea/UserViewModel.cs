namespace ClimbingCommunity.Web.ViewModels.AdminArea
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Role { get; set; } = null!;
    }
}
