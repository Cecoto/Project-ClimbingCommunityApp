namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ImageSereviceTests
    {
        private IImageService _imageService;

        [SetUp]
        public void Setup()
        {
            // Initialize the service, mock dependencies if needed
            _imageService = new ImageService();
        }

        [Test]
        public async Task Test_SavePictureAsync_ValidInput()
        {
            // Arrange
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(p => p.FileName).Returns("test.jpg");
            pictureMock.Setup(p => p.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            var dirName = "profile";

            // Act
            var result = await _imageService.SavePictureAsync(pictureMock.Object, dirName);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains($"/images/{dirName}/"));
        }

        [Test]
        public async Task Test_SavePictureAsync_DirectoryDoesNotExist()
        {
            // Arrange
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(p => p.FileName).Returns("test.jpg");
            pictureMock.Setup(p => p.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            var dirName = "nonexistent";

            // Act
            var result = await _imageService.SavePictureAsync(pictureMock.Object, dirName);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains($"/images/{dirName}/"));

            var expectedDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{dirName}");
            Assert.True(Directory.Exists(expectedDirectoryPath));
        }
        [Test]
        public async Task Test_SavePictureAsync_NullPicture()
        {
            // Arrange
            IFormFile picture = null;
            var dirName = "profile";

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _imageService.SavePictureAsync(picture, dirName);
            }, "Object reference not set to an instance of an object.");
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_SavePictureAsync_EmptyDirectory()
        {
            // Arrange
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(p => p.FileName).Returns("test.jpg");
            pictureMock.Setup(p => p.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            var dirName = string.Empty;

            // Act & Assert
           
               var result =  await _imageService.SavePictureAsync(pictureMock.Object, dirName);
           
            Assert.IsEmpty(result);
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_SavePictureAsync_ValidFileName()
        {
            // Arrange
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(p => p.FileName).Returns("test.jpg");
            pictureMock.Setup(p => p.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            var dirName = "profile";

            // Act
            var result = await _imageService.SavePictureAsync(pictureMock.Object, dirName);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains($"/images/{dirName}/"));
        }

        

    }
}
