﻿namespace ClimbingCommunity.Services.Tests
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
        [Test]
        public async Task Test_SavePictureAsync_FileSuccessfullySaved()
        {
            // Arrange
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(p => p.FileName).Returns("test.jpg");
            pictureMock.Setup(p => p.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            var dirName = "testDir";

            // Act
            var result = await _imageService.SavePictureAsync(pictureMock.Object, dirName);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains($"/images/{dirName}/"));

            var expectedFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{dirName}");
            Assert.True(Directory.Exists(expectedFilePath));

            var expectedFullPath = Path.Combine(expectedFilePath, result.Replace($"/images/{dirName}/", ""));
            Assert.True(File.Exists(expectedFullPath));
        }
        [Test]
        public void Test_DeletePicture_FileExists()
        {
            // Arrange
            var imagePath = "/images/test.jpg";
            var wwwrootPath = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(wwwrootPath, "wwwroot" + imagePath);

            // Create a dummy file
            File.WriteAllText(fullPath, "Test content");

            // Act
            _imageService.DeletePicture(imagePath);

            // Assert
            Assert.IsFalse(File.Exists(fullPath));
        }

        [Test]
        public void Test_DeletePicture_FileDoesNotExist()
        {
            // Arrange
            var imagePath = "/images/nonexistent.jpg";
            var wwwrootPath = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(wwwrootPath, "wwwroot" + imagePath);

            // Act
            _imageService.DeletePicture(imagePath);

            // Assert
            Assert.IsFalse(File.Exists(fullPath)); 
        }

        [Test]
        public void Test_DeletePicture_EmptyImagePath()
        {
            // Arrange
            var imagePath = ""; 

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _imageService.DeletePicture(imagePath); 
            });
        }

        [Test]
        public void Test_DeletePicture_NullImagePath()
        {
            // Arrange
            string imagePath = null; 

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _imageService.DeletePicture(imagePath); 
            });
        }

        [Test]
        public void Test_DeletePicture_InvalidImagePath()
        {
            // Arrange
            var imagePath = "/invalid-path.jpg"; 

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _imageService.DeletePicture(imagePath); 
            });
        }
        [Test]
        public async Task Test_SavePhotosAsync_EmptyList()
        {
            // Arrange
            var photos = new List<IFormFile>();

            // Act
            var result = await _imageService.SavePhotosAsync(photos);

            // Assert
            Assert.IsEmpty(result);
        }


        [Test]
        public async Task Test_SavePhotosAsync_FilesWithNoContent()
        {
            // Arrange
            var emptyFileMock = new Mock<IFormFile>();
            emptyFileMock.Setup(p => p.FileName).Returns("empty.jpg");
            emptyFileMock.Setup(p => p.Length).Returns(0);

            var photos = new List<IFormFile> { emptyFileMock.Object };

            // Act
            var result = await _imageService.SavePhotosAsync(photos);

            // Assert
            Assert.IsEmpty(result);
        }

    }
}
