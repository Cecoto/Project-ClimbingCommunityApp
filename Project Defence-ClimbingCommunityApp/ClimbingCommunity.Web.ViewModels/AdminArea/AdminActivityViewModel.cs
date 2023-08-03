namespace ClimbingCommunity.Web.ViewModels.AdminArea
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdminActivityViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
