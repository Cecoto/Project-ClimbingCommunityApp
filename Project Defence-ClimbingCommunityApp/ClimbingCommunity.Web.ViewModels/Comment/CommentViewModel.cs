namespace ClimbingCommunity.Web.ViewModels.Comment
{
    using ClimbingCommunity.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
    }
}
