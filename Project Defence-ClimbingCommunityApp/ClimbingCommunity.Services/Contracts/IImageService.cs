namespace ClimbingCommunity.Services.Contracts
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImageService
    {

        public Task<string> SaveProfilePictureAsync(IFormFile profilePicture);
    }
}
