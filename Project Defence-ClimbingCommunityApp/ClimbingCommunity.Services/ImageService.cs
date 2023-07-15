namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Http;

    using System;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {

        public async Task<string> SavePictureAsync(IFormFile profilePicture, string dirName)
        {


            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);

            string profilePicturesDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{dirName}");

            if (!Directory.Exists(profilePicturesDirectory))
            {
                Directory.CreateDirectory(profilePicturesDirectory);
            }

            string filePath = Path.Combine(profilePicturesDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            return $"/images/{dirName}/" + uniqueFileName;
        }

        public void DeletePicture(string imagePath)
        {
            string wwwrootPath = Directory.GetCurrentDirectory();

            string fullPath = Path.Combine(wwwrootPath,"wwwroot" + imagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

        }
    }
}
