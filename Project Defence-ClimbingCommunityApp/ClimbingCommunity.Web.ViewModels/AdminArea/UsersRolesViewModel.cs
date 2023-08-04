namespace ClimbingCommunity.Web.ViewModels.AdminArea
{
    public class UsersRolesViewModel
    {
        public UsersRolesViewModel()
        {
            this.UsersEmails = new HashSet<string>();
            this.RoleNames = new HashSet<string>();
        }
        public IEnumerable<string> UsersEmails { get; set; }
        public IEnumerable<string> RoleNames { get; set; }
    }
}
