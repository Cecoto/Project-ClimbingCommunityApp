namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    public class CommentService : ICommentService
    {
        private readonly IRepository repo;
        public CommentService(IRepository _repo)
        {
            repo = _repo;
        }
        public async Task<ICollection<CommentViewModel>> GetAllCommentsByTripId(string tripId)
        {
            return await repo.AllReadonly<Comment>(c => c.ClimbingTripId == Guid.Parse(tripId))
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    AuthorId = c.AuthorId,
                    Author = c.Author
                })
                .ToListAsync();
        }
    }
}
