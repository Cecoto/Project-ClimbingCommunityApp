namespace ClimbingCommunity.Services
{
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        public async Task<string> SaveProfilePictureAsync(IFormFile profilePicture)
        {

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);

            string profilePicturesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "images/ProfilePictures");

            if (!Directory.Exists(profilePicturesDirectory))
            {
                Directory.CreateDirectory(profilePicturesDirectory);
            }

            string filePath = Path.Combine(profilePicturesDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            return "ProfilePictures/" + uniqueFileName;
        }
    }
}
