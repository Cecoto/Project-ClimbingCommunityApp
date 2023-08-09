namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    public class TrainingServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IImageService> _imageServiceMock;
        private TrainingService _trainingService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _imageServiceMock = new Mock<IImageService>();
            _trainingService = new TrainingService(_repositoryMock.Object, _imageServiceMock.Object);

        }
        [Test]
        public async Task Test_CreateAsync_WithPhoto()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 1,
                PhotoFile = new Mock<IFormFile>().Object
            };
            var savedPhotoUrl = "/images/Trainings/test.jpg";

            _imageServiceMock.Setup(service => service.SavePictureAsync(It.IsAny<IFormFile>(), "Trainings"))
                             .ReturnsAsync(savedPhotoUrl);

            // Act
            await _trainingService.CreateAsync(organizatorId, model);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync<Training>(It.IsAny<Training>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.That(model.PhotoUrl, Is.EqualTo(savedPhotoUrl));

        }

        [Test]
        public async Task Test_CreateAsync_WithoutPhoto()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 1
            };

            // Act
            await _trainingService.CreateAsync(organizatorId, model);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync<Training>(It.IsAny<Training>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsNull(model.PhotoUrl);
        }
        [Test]
        public async Task Test_CreateAsync_NullModel()
        {
            // Arrange
            var organizatorId = "organizator1";
            TrainingFormViewModel model = null;

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.CreateAsync(organizatorId, model);
            }, "Object reference not set to an instance of an object.");


            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_CreateAsync_EmptyTitle()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 1
            };

            // Act & Assert
            await _trainingService.CreateAsync(organizatorId, model);

        }

        [Test]
        public async Task Test_CreateAsync_InvalidPrice()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = -10, // Negative price
                Location = "Test Location",
                TragetId = 1
            };

            // Act & Assert

            await _trainingService.CreateAsync(organizatorId, model);

        }

        [Test]
        public async Task Test_CreateAsync_InvalidTargetId()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 0 // Invalid target ID
            };

            await _trainingService.CreateAsync(organizatorId, model);

        }
        [Test]
        public async Task Test_CreateAsync_NullOrganizatorId()
        {
            // Arrange
            string organizatorId = null;
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 1
            };

            // Act
            await _trainingService.CreateAsync(organizatorId, model);
        }

        [Test]
        public async Task Test_CreateAsync_NullModelWithNullOrganizatorId()
        {
            // Arrange
            string organizatorId = null;
            TrainingFormViewModel model = null;

            // Act
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.CreateAsync(organizatorId, model);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_CreateAsync_NullPhotoFile()
        {
            // Arrange
            var organizatorId = "organizator1";
            var model = new TrainingFormViewModel
            {
                Title = "Test Training",
                Duration = 3,
                Price = 50,
                Location = "Test Location",
                TragetId = 1,
                PhotoFile = null
            };

            // Act
            await _trainingService.CreateAsync(organizatorId, model);


        }
        [Test]
        public async Task Test_DeleteTrainingByIdAsync_ValidTrainingId()
        {
            // Arrange
            var trainingId = "479334bf-b9e3-4661-b45a-14c0917f0411";
            var training = new Training
            {
                Id = Guid.Parse(trainingId),
                Title = "Training Title",
                isActive = true
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(training);

            // Act
            await _trainingService.DeleteTrainingByIdAsync(trainingId);

            // Assert
            Assert.IsFalse(training.isActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_DeleteTrainingByIdAsync_InvalidTrainingId()
        {
            // Arrange
            var trainingId = "invalid id";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null!);

            // Act
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.DeleteTrainingByIdAsync(trainingId);

            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }


    }
}
