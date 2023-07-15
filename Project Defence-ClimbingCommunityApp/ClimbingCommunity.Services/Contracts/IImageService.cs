namespace ClimbingCommunity.Services.Contracts
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImageService
    {

        Task<string> SavePictureAsync(IFormFile profilePicture, string dirName);

        void DeletePicture(string imagePath);

        Task<List<string>> SavePhotosAsync(List<IFormFile> photos);
    }
}
