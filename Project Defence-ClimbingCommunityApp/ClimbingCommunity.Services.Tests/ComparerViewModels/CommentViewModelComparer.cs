namespace ClimbingCommunity.Services.Tests.ComparerViewModels
{
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CommentViewModelComparer : IComparer, IComparer<CommentViewModel>
    {
        public int Compare(object x, object y)
        {
            if (x is CommentViewModel xModel && y is CommentViewModel yModel)
            {
                return Compare(xModel, yModel);
            }

            return -1;
        }
        public int Compare(CommentViewModel x, CommentViewModel y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;


            int idComparison = x.Id.CompareTo(y.Id);
            if (idComparison != 0)
                return idComparison;

            int titleComparison = string.Compare(x.Text, y.Text, StringComparison.OrdinalIgnoreCase);
            if (titleComparison != 0)
                return titleComparison;

            int photoUrlComparison = string.Compare(x.AuthorId, y.AuthorId, StringComparison.OrdinalIgnoreCase);
            if (photoUrlComparison != 0)
                return photoUrlComparison;

            int createdOnComparison = x.CreatedOn.CompareTo(y.CreatedOn);
            if (createdOnComparison != 0)
                return createdOnComparison;

            return 0;
        }
    }
}
