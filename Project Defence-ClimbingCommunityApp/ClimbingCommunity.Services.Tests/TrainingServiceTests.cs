namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Data.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Services.Tests.ComparerViewModels;
    using ClimbingCommunity.Services.Tests.Mocking;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.Training;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

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

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(It.IsAny<Guid>()))
                           .ThrowsAsync(new FormatException("Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)."));

            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.DeleteTrainingByIdAsync(trainingId);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_DeleteTrainingByIdAsync_NullTraining()
        {
            // Arrange
            var trainingId = "479334bf-b9e3-4661-b45a-14c0917f0411";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null);

            // Act
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _trainingService.DeleteTrainingByIdAsync(null);

            });

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task Test_DeleteTrainingByIdAsync_InactiveTraining()
        {
            // Arrange
            var trainingId = "479334bf-b9e3-4661-b45a-14c0917f0411";
            var training = new Training
            {
                Id = Guid.Parse(trainingId),
                Title = "Training Title",
                isActive = false
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
        public async Task Test_DeleteTrainingByIdAsync_ActiveTraining()
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
        public async Task Test_EditTrainingByIdAsync_UpdateExistingTraining()
        {
            // Arrange
            var trainingId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var model = new TrainingFormViewModel
            {
                Title = "Updated Training Title",
                Duration = 2,
                Price = 25.0m,
                Location = "Updated Location",
                TragetId = 2,
                PhotoFile = null // No new photo
            };

            var existingTraining = new Training
            {
                Id = Guid.Parse(trainingId),
                Title = "Original Training Title",
                Duration = 3,
                Price = 40.0m,
                Location = "Original Location",
                TargetId = 1,
                PhotoUrl = "photo1.jpg"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(existingTraining);

            // Act
            await _trainingService.EditTrainingByIdAsync(trainingId, model);

            // Assert
            Assert.That(existingTraining.Title, Is.EqualTo(model.Title));
            Assert.That(existingTraining.Duration, Is.EqualTo(model.Duration));
            Assert.That(existingTraining.Price, Is.EqualTo(model.Price));
            Assert.That(existingTraining.Location, Is.EqualTo(model.Location));
            Assert.That(existingTraining.TargetId, Is.EqualTo(model.TragetId));
            Assert.That(existingTraining.PhotoUrl, Is.EqualTo("photo1.jpg"));

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_EditTrainingByIdAsync_UpdateExistingTrainingWithNewPhoto()
        {
            // Arrange
            var trainingId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var model = new TrainingFormViewModel
            {
                Title = "Updated Training Title",
                Duration = 2,
                Price = 20.0m,
                Location = "Updated Location",
                TragetId = 2,
                PhotoFile = new Mock<IFormFile>().Object // Simulating new photo
            };

            var existingTraining = new Training
            {
                Id = Guid.Parse(trainingId),
                Title = "Original Training Title",
                Duration = 3,
                Price = 40.0m,
                Location = "Original Location",
                TargetId = 1,
                PhotoUrl = "photo1.jpg"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(existingTraining);

            // Act
            await _trainingService.EditTrainingByIdAsync(trainingId, model);

            // Assert
            Assert.That(existingTraining.Title, Is.EqualTo(model.Title));
            Assert.That(existingTraining.Duration, Is.EqualTo(model.Duration));
            Assert.That(existingTraining.Price, Is.EqualTo(model.Price));
            Assert.That(existingTraining.Location, Is.EqualTo(model.Location));
            Assert.That(existingTraining.TargetId, Is.EqualTo(model.TragetId));
            Assert.That(existingTraining.PhotoUrl, Is.Not.EqualTo("photo1.jpg")); // Photo should be updated

            _imageServiceMock.Verify(imgService => imgService.DeletePicture("photo1.jpg"), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_EditTrainingByIdAsync_UpdateNonExistingTraining()
        {
            // Arrange
            var trainingId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var model = new TrainingFormViewModel
            {
                Title = "Updated Training Title",
                Duration = 2,
                Price = 15.0m,
                Location = "Updated Location",
                TragetId = 2,
                PhotoFile = null // No new photo
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null!);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.EditTrainingByIdAsync(trainingId, model);
            }, "Object reference not set to an instance of an object.");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_EditTrainingByIdAsync_InvalidTrainingId()
        {
            // Arrange
            var trainingId = "invalid id";
            var model = new TrainingFormViewModel
            {
                Title = "Updated Training Title",
                Duration = 2,
                Price = 25.0m,
                Location = "Updated Location",
                TragetId = 2,
                PhotoFile = null // No new photo
            };

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.EditTrainingByIdAsync(trainingId, model);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;

        }
        [Test]
        public async Task Test_EditTrainingByIdAsync_NullModel()
        {
            // Arrange
            var trainingId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";

            var existingTraining = new Training
            {
                Id = Guid.Parse(trainingId),
                Title = "Original Training Title",
                Duration = 3,
                Price = 50.0m,
                Location = "Original Location",
                TargetId = 1,
                PhotoUrl = "photo1.jpg"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(existingTraining);

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.EditTrainingByIdAsync(trainingId, null);
            }, "Object reference not set to an instance of an object.");

            // No interactions with repository or imageService should occur
            _imageServiceMock.Verify(imgService => imgService.DeletePicture(It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetAllJoinedTrainingByUserIdAsync_ValidUserId()
        {
            // Arrange
            var userId = "3bf16936-2071-4762-b6aa-07524977acd6";
            var trainingData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true && t.JoinedClimbers.Any(c => c.ClimberId == userId)));

            var expectedViewModels = trainingData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => (t.isActive == true || t.isActive == null) && t.JoinedClimbers.Any(c => c.ClimberId == userId))
                .Select(t => new JoinedTrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Organizator = t.Coach,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedTrainingViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllJoinedTrainingByUserIdAsync_InvalidUserId()
        {
            // Arrange
            var userId = "0ec7ef62-108a-4e04-8cd3-823a0ee78fab";
            var joinedTrainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(joinedTrainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true && t.JoinedClimbers.Any(c => c.ClimberId == userId)));

            // Act
            var result = await _trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllJoinedTrainingByUserIdAsync_NoJoinedTrainings()
        {
            // Arrange
            var userId = "8f3553fd-8607-412b-8318-512070cf8789";
            var joinedTrainingsData = TrainingsForTests(); // Empty list of joined trainings

            var mockQueryable = new MockAsyncEnumerable<Training>(joinedTrainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.JoinedClimbers.Any(c => c.ClimberId == userId)));

            // Act
            var result = await _trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllJoinedTrainingByUserIdAsync_MixedActiveAndInactiveTrainings()
        {
            // Arrange
            var userId = "user-with-mixed-trainings";
            var joinedTrainingsData = TrainingsForTests();
            joinedTrainingsData[0].isActive = false;

            var mockQueryable = new MockAsyncEnumerable<Training>(joinedTrainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => (t.isActive == true || t.isActive == null) && t.JoinedClimbers.Any(c => c.ClimberId == userId)));

            IEnumerable<JoinedTrainingViewModel> expectedViewModels = joinedTrainingsData
                .Where(t => (t.isActive == true || t.isActive == null) && t.JoinedClimbers.Any(c => c.ClimberId == userId))
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new JoinedTrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Organizator = t.Coach,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedTrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllJoinedTrainingByUserIdAsync_EmptyData()
        {
            // Arrange
            var userId = "user-with-empty-data";
            var joinedTrainingsData = new List<Training>(); // Empty list of joined trainings

            var mockQueryable = new MockAsyncEnumerable<Training>(joinedTrainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn));

            // Act
            var result = await _trainingService.GetAllJoinedTrainingByUserIdAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllTargetsAsync_ReturnsTargets()
        {
            // Arrange
            var targetsData = TargetsForTesting();
            var mockQueryable = new MockAsyncEnumerable<Target>(targetsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Target>())
                           .Returns(mockQueryable);

            IEnumerable<TargetViewModel> expectedViewModels = targetsData
                .Select(t => new TargetViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                });

            // Act
            var result = await _trainingService.GetAllTargetsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TargetViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllTargetsAsync_EmptyTargets()
        {
            //Arrange
            var targetsData = new List<Target>();
            var mockQueryable = new MockAsyncEnumerable<Target>(targetsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Target>())
                           .Returns(mockQueryable);

            // Act
            var result = await _trainingService.GetAllTargetsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllTargetsAsync_MultipleTargets()
        {
            // Arrange
            var targetsData = TargetsForTesting();
            var mockQueryable = new MockAsyncEnumerable<Target>(targetsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Target>())
                           .Returns(mockQueryable);

            // Act
            var result = await _trainingService.GetAllTargetsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(targetsData.Count));
        }
        [Test]
        public async Task Test_GetAllTrainingsAsync_ReturnsTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true || t.isActive == false));

            var expectedViewModels = trainingsData
                .Where(t => t.isActive == true || t.isActive == null)
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsAsync_NoTrainingsAvailable()
        {
            // Arrange
            var trainingData = new List<Training>();
            var mockedQueryable = new MockAsyncEnumerable<Training>(trainingData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                        .Returns(mockedQueryable);

            // Act
            var result = await _trainingService.GetAllTrainingsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllTrainingsAsync_ActiveTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = true;
            trainingsData[1].isActive = true;
            trainingsData[2].isActive = true;
            trainingsData[3].isActive = true;

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => t.isActive == true || t.isActive == null)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsAsync_AllTrainingsAreInactive()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = false;
            trainingsData[1].isActive = false;
            trainingsData[2].isActive = false;
            trainingsData[3].isActive = false;

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true));

            // Act
            var result = await _trainingService.GetAllTrainingsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllTrainingsAsync_WithMixedStatus()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = true;
            trainingsData[1].isActive = true;
            trainingsData[2].isActive = false;
            trainingsData[3].isActive = false;

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => t.isActive == true || t.isActive == null)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_TitleMatch()
        {
            // Arrange
            var searchText = "Training";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => (t.isActive == true || t.isActive == null) && t.Title.Contains(searchText)));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => (t.isActive == true || t.isActive == null) && t.Title.Contains(searchText))
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_CoachFirstNameMatch()
        {
            // Arrange
            var searchText = "John";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => (t.isActive == true || t.isActive == null) && t.Coach.FirstName.Contains(searchText)));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => (t.isActive == true || t.isActive == null) && t.Coach.FirstName.Contains(searchText))
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_CoachLastNameMatch()
        {
            // Arrange
            var searchText = "Ivan";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true || t.isActive == null)
                           .Where(t => t.Coach.LastName.Contains(searchText)));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => t.isActive == true || t.isActive == null)
                .Where(t => t.Coach.LastName.Contains(searchText))
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_TargetMatch()
        {
            // Arrange
            var searchText = "Endurance";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => (t.isActive == true || t.isActive == null) && t.Target.Name.Contains(searchText)));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => (t.isActive == true || t.isActive == null) && t.Target.Name.Contains(searchText))
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_NotFoundLocationReturnsEmptyCollection()
        {
            // Arrange
            var searchText = "France";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true && t.Location.Contains(searchText)));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => (t.isActive == true || t.isActive == null) && t.Location.Contains(searchText))
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false,
                    Organizator = t.Coach,
                    Price = t.Price,
                    NumberOfParticipants = t.JoinedClimbers.Count()
                });

            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsByStringAsync_NoMatch()
        {
            // Arrange
            var searchText = "NonExistent";
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
            .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.Title.Contains(searchText) ||
                            t.Coach.FirstName.Contains(searchText) ||
                            t.Coach.LastName.Contains(searchText) ||
                            t.Target.Name.Contains(searchText) ||
                            t.Location.Contains(searchText)));


            var expectedViewModels = trainingsData
                 .OrderByDescending(t => t.CreatedOn)
                 .Where(t => t.Title.Contains(searchText) ||
                        t.Coach.FirstName.Contains(searchText) ||
                        t.Coach.LastName.Contains(searchText) ||
                        t.Target.Name.Contains(searchText) ||
                        t.Location.Contains(searchText))
                 .Select(t => new TrainingViewModel
                 {
                     Id = t.Id.ToString(),
                     Title = t.Title,
                     PhotoUrl = t.PhotoUrl,
                     Location = t.Location,
                     OrganizatorId = t.CoachId,
                     Duration = t.Duration,
                     Target = t.Target.Name,
                     isOrganizator = false,
                     Organizator = t.Coach,
                     Price = t.Price,
                     NumberOfParticipants = t.JoinedClimbers.Count()
                 });
            // Act
            var result = await _trainingService.GetAllTrainingsByStringAsync(searchText);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllTrainingsForAdminAsync()
        {
            // Arrange
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.All<Training>())
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new AdminActivityViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    CreatedOn = t.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = t.isActive,
                    Location = t.Location
                });

            // Act
            var result = await _trainingService.GetAllTrainingsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }

        // Additional scenarios
        [Test]
        public async Task Test_GetAllTrainingsForAdminAsync_EmptyList()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.All<Training>())
                           .Returns(new MockAsyncEnumerable<Training>(new List<Training>()));

            // Act
            var result = await _trainingService.GetAllTrainingsForAdminAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllTrainingsForAdminAsync_ActiveAndInactiveTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.All<Training>())
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new AdminActivityViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    CreatedOn = t.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = t.isActive,
                    Location = t.Location
                });

            // Act
            var result = await _trainingService.GetAllTrainingsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsForAdminAsync_GetAllInactiveTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = false;
            trainingsData[1].isActive = false;
            trainingsData[2].isActive = false;
            trainingsData[3].isActive = false;
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.All<Training>())
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == false));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Where(t => t.isActive == false)
                .Select(t => new AdminActivityViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    CreatedOn = t.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = t.isActive,
                    Location = t.Location
                });

            // Act
            var result = await _trainingService.GetAllTrainingsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTrainingsForAdminAsync_GetAllMixedStatusTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = true;
            trainingsData[1].isActive = false;
            trainingsData[2].isActive = true;
            trainingsData[3].isActive = false;
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.All<Training>())
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn));

            var expectedViewModels = trainingsData
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new AdminActivityViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    CreatedOn = t.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = t.isActive,
                    Location = t.Location
                });

            // Act
            var result = await _trainingService.GetAllTrainingsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeTrainingsAsync_MultipleTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true || t.isActive == null)
                           .Take(3));

            var expectedViewModels = trainingsData
                .Where(t => t.isActive == true || t.isActive == null)
                .OrderByDescending(t => t.CreatedOn)
                .Take(3)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _trainingService.GetLastThreeTrainingsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetLastThreeTrainingsAsync_LessThanThreeTrainingsBecauseOtherInActive()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[3].isActive = false;
            trainingsData[2].isActive = false;
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true || t.isActive == null));

            var expectedViewModels = trainingsData
                .Where(t => t.isActive == true || t.isActive == null)
                .OrderByDescending(t => t.CreatedOn)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    OrganizatorId = t.CoachId,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _trainingService.GetLastThreeTrainingsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetLastThreeTrainingsAsync_NoTrainings()
        {
            // Arrange
            var trainingsData = new List<Training>();
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(t => t.CreatedOn));

            // Act
            var result = await _trainingService.GetLastThreeTrainingsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetLastThreeTrainingsAsync_AllInactiveTrainings()
        {
            // Arrange
            var trainingsData = TrainingsForTests();

            trainingsData.ForEach(t => t.isActive = false);

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true));

            // Act
            var result = await _trainingService.GetLastThreeTrainingsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetLastThreeTrainingsAsync_AllTrainingsWithMixedStatus()
        {
            // Arrange
            var trainingsData = TrainingsForTests();
            trainingsData[0].isActive = true;
            trainingsData[1].isActive = false;
            trainingsData[2].isActive = true;
            trainingsData[3].isActive = false;
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(t => t.CreatedOn)
                           .Where(t => t.isActive == true));

            int expectedCount = trainingsData.Count(t => t.isActive == true);
            // Act
            var result = await _trainingService.GetLastThreeTrainingsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }
        [Test]
        public async Task Test_GetMyTrainingsByIdAsync_CoachWithTrainings()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";

            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .Where(t => (t.isActive == true || t.isActive == null) && t.CoachId == userId));

            var expectedViewModels = trainingsData
                .Where(t => (t.isActive == true || t.isActive == null) && t.CoachId == userId)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = true,
                    Price = t.Price
                });

            // Act
            var result = await _trainingService.GetMyTrainingsByIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetMyTrainingsByIdAsync_CoachWithoutTrainings()
        {
            // Arrange
            var userId = "7383f815-8f0f-4916-81b2-2a05914ef4f3"; // id of a coach which don't have trainings

            var trainingsData = TrainingsForTests();

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .Where(t => t.CoachId == userId));

            // Act
            var result = await _trainingService.GetMyTrainingsByIdAsync(userId);

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetMyTrainingsByIdAsync_CoachWithInactiveTrainings()
        {
            // Arrange
            var userId = "e81ccf9c-35aa-4adb-a23e-249e7096000e"; // Coach Id
            var trainingsData = TrainingsForTests().Where(t => t.CoachId == userId).ToList();

            trainingsData.ForEach(t => t.isActive = false);
            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .Where(t => t.isActive == true));


            // Act
            var result = await _trainingService.GetMyTrainingsByIdAsync(userId);

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetMyTrainingsByIdAsync_CoachWithInactiveAndActiveTrainings()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";

            var trainingsData = TrainingsForTests();

            trainingsData[0].isActive = true;
            trainingsData[1].isActive = true;
            trainingsData[2].isActive = false;
            trainingsData[3].isActive = false;

            var mockQueryable = new MockAsyncEnumerable<Training>(trainingsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Training>(It.IsAny<Expression<Func<Training, bool>>>()))
                           .Returns(mockQueryable
                           .Where(t => t.isActive == true && t.CoachId == userId));


            var expectedViewModels = trainingsData
                .Where(t => t.isActive == true && t.CoachId == userId)
                .Select(t => new TrainingViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    PhotoUrl = t.PhotoUrl,
                    Location = t.Location,
                    Duration = t.Duration,
                    Target = t.Target.Name,
                    isOrganizator = true,
                    Price = t.Price,
                    OrganizatorId = t.CoachId
                });

            // Act
            var result = await _trainingService.GetMyTrainingsByIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TrainingViewModelComparer());
        }

        [Test]
        public async Task Test_GetTrainingForEditByIdAsync_ValidTrainingId()
        {
            // Arrange

            var trainingId = "d70cbeed-8aa8-4465-9bce-31c2ce73b455";

            var trainingData = TrainingsForTests().FirstOrDefault(t => t.Id == Guid.Parse(trainingId));

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(trainingData!);

            var expectedViewModel = new TrainingFormViewModel()
            {
                Title = trainingData.Title,
                PhotoUrl = trainingData.PhotoUrl,
                Location = trainingData.Location,
                IsEditModel = true,
                Duration = trainingData.Duration,
                Price = trainingData.Price,
                OrganizatorId = trainingData.CoachId,
                TragetId = trainingData.TargetId
            };

            // Act
            var result = await _trainingService.GetTrainingForEditByIdAsync(trainingId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new TrainingFormViewModelComparer()));
        }

        [Test]
        public async Task Test_GetTrainingForEditByIdAsync_InvalidTrainingId()
        {
            // Arrange
            var trainingId = "e3fbb1af-674c-4220-9f6e-58a4d350d4e4";

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.GetTrainingForEditByIdAsync(trainingId);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetTrainingForEditByIdAsync_EmptyTrainingId()
        {
            // Arrange
            string trainingId = string.Empty;

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.GetTrainingForEditByIdAsync(trainingId);
            }, "Unrecognized Guid format.");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetTrainingForEditByIdAsync_DifferentTrainingId()
        {
            // Arrange
            var trainingId = "17cc72da-f2a2-4af8-bb67-3699db13fb1b";

            var trainingData = TrainingsForTests().FirstOrDefault(t => t.Id == Guid.Parse(trainingId));

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(trainingData!);

            var expectedViewModel = new TrainingFormViewModel()
            {
                Title = trainingData.Title,
                PhotoUrl = trainingData.PhotoUrl,
                Location = trainingData.Location,
                IsEditModel = true,
                Duration = trainingData.Duration,
                Price = trainingData.Price,
                OrganizatorId = trainingData.CoachId,
                TragetId = trainingData.TargetId
            };

            // Act
            var result = await _trainingService.GetTrainingForEditByIdAsync(trainingId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new TrainingFormViewModelComparer()));
        }
        [Test]
        public async Task Test_GetTrainingForEditByIdAsync_VariousModelProperties()
        {
            // Arrange
            var trainingId = "17cc72da-f2a2-4af8-bb67-3699db13fb1b";

            var trainingData = TrainingsForTests().FirstOrDefault(t => t.Id == Guid.Parse(trainingId));

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(trainingData!);

            // Act
            var result = await _trainingService.GetTrainingForEditByIdAsync(trainingId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(trainingData.Title));
            Assert.That(result.PhotoUrl, Is.EqualTo(trainingData.PhotoUrl));
            Assert.That(result.Location, Is.EqualTo(trainingData.Location));
            Assert.IsTrue(result.IsEditModel);
            Assert.That(result.Duration, Is.EqualTo(trainingData.Duration));
            Assert.That(result.Price, Is.EqualTo(trainingData.Price));
            Assert.That(result.OrganizatorId, Is.EqualTo(trainingData.CoachId));
            Assert.That(result.TragetId, Is.EqualTo(trainingData.TargetId));
        }
        [Test]
        public async Task Test_IsTargetExistsByIdAsync_TargetExists()
        {
            // Arrange
            var targetId = 1;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Target>(targetId))
                           .ReturnsAsync(new Target());

            // Act
            var result = await _trainingService.IsTargetExistsByIdAsync(targetId);

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task Test_IsTargetExistsByIdAsync_TargetDoesNotExist()
        {
            // Arrange
            var targetId = 5;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Target>(targetId))
                           .ReturnsAsync((Target)null);

            // Act
            var result = await _trainingService.IsTargetExistsByIdAsync(targetId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsTargetExistsByIdAsync_NegativeTargetId()
        {
            // Arrange
            var targetId = -1;

            // Act & Assert

            await _trainingService.IsTargetExistsByIdAsync(targetId);

        }
        [Test]
        public async Task Test_IsTargetExistsByIdAsync_ZeroTargetId()
        {
            // Arrange
            var targetId = 0;

            // Act & Assert
            await _trainingService.IsTargetExistsByIdAsync(targetId);
        }
        [Test]
        public async Task Test_IsTargetExistsByIdAsync_LargeTargetId()
        {
            // Arrange
            var targetId = 1000000;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Target>(targetId))
                           .ReturnsAsync((Target)null);

            // Act
            var result = await _trainingService.IsTargetExistsByIdAsync(targetId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsTrainingExistsByIdAsync_TrainingExists()
        {
            // Arrange
            var trainingId = "8388d571-d7cf-4ca8-8065-c2f7c1224e98";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(new Training());

            // Act
            var result = await _trainingService.IsTrainingExistsByIdAsync(trainingId);

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task Test_IsTrainingExistsByIdAsync_TrainingDoesNotExist()
        {
            // Arrange
            var trainingId = "17cc72da-f2a2-4af8-bb67-3699db13fb12"; // non exist id

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null);

            // Act
            var result = await _trainingService.IsTrainingExistsByIdAsync(trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsTrainingExistsByIdAsync_InvalidTrainingIdFormat()
        {
            // Arrange
            var trainingId = "invalidId"; // Use an invalid training ID format

            // Act
            var result = await _trainingService.IsTrainingExistsByIdAsync(trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsTrainingExistsByIdAsync_NullTrainingId()
        {
            // Arrange
            string trainingId = null; // Use a null training ID

            // Act
            var result = await _trainingService.IsTrainingExistsByIdAsync(trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsTrainingExistsByIdAsync_EmptyTrainingId()
        {
            // Arrange
            var trainingId = ""; // Use an empty training ID

            // Act
            var result = await _trainingService.IsTrainingExistsByIdAsync(trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTrainingByIdAsync_UserIsOrganizator()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var trainingId = "8388d571-d7cf-4ca8-8065-c2f7c1224e98";

            var trainingData = TrainingsForTests().FirstOrDefault(t => t.Id == Guid.Parse(trainingId));


            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))!
                           .ReturnsAsync(trainingData);

            // Act
            var result = await _trainingService.IsUserOrganizatorOfTrainingByIdAsync(userId, trainingId);

            // Assert
            Assert.True(result);
        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTrainingByIdAsync_UserIsNotOrganizator()
        {
            // Arrange
            var userId = "e81ccf9c-35aa-4adb-a23e-249e7096000e";
            var trainingId = "8388d571-d7cf-4ca8-8065-c2f7c1224e98";
            var trainingData = TrainingsForTests().FirstOrDefault(t => t.Id == Guid.Parse(trainingId));

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))!
                           .ReturnsAsync(trainingData);

            // Act
            var result = await _trainingService.IsUserOrganizatorOfTrainingByIdAsync(userId, trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTrainingByIdAsync_NonExistentTraining()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var trainingId = "e3fbb1af-674c-4220-9f6e-58a4d350d4e4";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null!);

            // Act and Assert

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _trainingService.IsUserOrganizatorOfTrainingByIdAsync(userId, trainingId);

            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTrainingByIdAsync_InvalidTrainingIdFormat()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var trainingId = "invalidId";

            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.IsUserOrganizatorOfTrainingByIdAsync(userId, trainingId);

            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;

        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTrainingByIdAsync_NullTrainingId()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            string trainingId = null;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _trainingService.IsUserOrganizatorOfTrainingByIdAsync(userId, trainingId);

            }, "Value cannot be null. (Parameter 'input')");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_IsUserParticipateInTrainingByIdAsync_UserParticipates()
        {
            // Arrange
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";
            var trainingId = "17cc72da-f2a2-4af8-bb67-3699db13fb1b";

            var trainingClimberData = new TrainingClimber
            {
                ClimberId = userId,
                TrainingId = Guid.Parse(trainingId)
            };

            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TrainingClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync(trainingClimberData);

            // Act
            var result = await _trainingService.IsUserParticipateInTrainingByIdAsync(userId, trainingId);

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTrainingByIdAsync_UserDoesNotParticipate()
        {
            // Arrange
            var userId = "3bf16936-2071-4762-b6aa-07524977acd6";
            var trainingId = "d70cbeed-8aa8-4465-9bce-31c2ce73b455";

            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TrainingClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync((TrainingClimber)null!);

            // Act
            var result = await _trainingService.IsUserParticipateInTrainingByIdAsync(userId, trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsUserParticipateInTrainingByIdAsync_NonExistentTraining()
        {
            // Arrange
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";
            var trainingId = "e3fbb1af-674c-4220-9f6e-58a4d350d4e4";

            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TrainingClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync((TrainingClimber)null!);

            // Act
            var result = await _trainingService.IsUserParticipateInTrainingByIdAsync(userId, trainingId);

            // Assert
            Assert.False(result);
        }
        [Test]
        public async Task Test_IsUserParticipateInTrainingByIdAsync_InvalidTrainingIdFormat()
        {
            // Arrange
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05"; // User ID
            var trainingId = "invalidId"; // Use an invalid training ID format

            // Act and Assert

            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.IsUserParticipateInTrainingByIdAsync(userId, trainingId);

            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_IsUserParticipateInTrainingByIdAsync_NullTrainingId()
        {
            // Arrange
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";
            string trainingId = null;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _trainingService.IsUserParticipateInTrainingByIdAsync(userId, trainingId);

            }, "Value cannot be null. (Parameter 'input')");

            await Task.CompletedTask;

        }
        [Test]
        public async Task Test_JoinTrainingAsync_SuccessfulJoin()
        {
            // Arrange
            var trainingId = "8388d571-d7cf-4ca8-8065-c2f7c1224e98";
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";

            _repositoryMock.Setup(repo => repo.AddAsync<TrainingClimber>(It.IsAny<TrainingClimber>()))
                           .Callback<TrainingClimber>(tc => tc.TrainingId = Guid.Parse(trainingId))
                           .Returns(Task.CompletedTask);

            // Act
            await _trainingService.JoinTrainingAsync(trainingId, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_JoinTrainingAsync_InvalidTrainingIdFormat()
        {
            // Arrange
            var trainingId = "invalidId";
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.JoinTrainingAsync(trainingId, userId);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_JoinTrainingAsync_NullTrainingId()
        {
            // Arrange
            string trainingId = null;
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _trainingService.JoinTrainingAsync(trainingId, userId);
            }, "Value cannot be null.");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_JoinTrainingAsync_InvalidUserIdFormat()
        {
            // Arrange
            var trainingId = "d70cbeed-8aa8-4465-9bce-31c2ce73b455";
            var userId = "invalidId";

            // Act & Assert

            await _trainingService.JoinTrainingAsync(trainingId, userId);

        }
        [Test]
        public async Task Test_LeaveTrainingAsync_SuccessfulLeave()
        {
            // Arrange
            var trainingId = "17cc72da-f2a2-4af8-bb67-3699db13fb1b";
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";

            var trainingClimberToDelete = new TrainingClimber
            {
                ClimberId = userId,
                TrainingId = Guid.Parse(trainingId)
            };

            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TrainingClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync(trainingClimberToDelete);

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<TrainingClimber>()))
                           .Returns(Task.CompletedTask);

            // Act
            await _trainingService.LeaveTrainingAsync(trainingId, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_LeaveTrainingAsync_NonExistentTrainingClimber()
        {
            // Arrange
            var trainingId = "d70cbeed-8aa8-4465-9bce-31c2ce73b455"; 
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05"; 

            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TrainingClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync((TrainingClimber)null!);

            // Act
            await _trainingService.LeaveTrainingAsync(trainingId, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_LeaveTrainingAsync_InvalidTrainingIdFormat()
        {
            // Arrange
            var trainingId = "invalidId"; 
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05";

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _trainingService.LeaveTrainingAsync(trainingId, userId);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_LeaveTrainingAsync_NullTrainingId()
        {
            // Arrange
            string trainingId = null; 
            var userId = "62764ec6-e266-4528-ae51-e44da8343b05"; 

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _trainingService.LeaveTrainingAsync(trainingId, userId);
            }, "Value cannot be null.");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_LeaveTrainingAsync_InvalidUserIdFormat()
        {
            // Arrange
            var trainingId = "d70cbeed-8aa8-4465-9bce-31c2ce73b455"; 
            var userId = "invalidId"; 

            // Act & Assert
            
                await _trainingService.LeaveTrainingAsync(trainingId, userId);
           
        }



        private List<Target> TargetsForTesting()
        {
            return new List<Target>
             {
             new Target { Id = 1, Name = "Strength"},
             new Target { Id = 2, Name = "Endurance"},
             new Target { Id = 3, Name = "Flexibility"},
             new Target { Id = 4, Name  = "Power" }
             };
        }
        private List<Training> TrainingsForTests()
        {
            var trainingsData = new List<Training>
        {
        new Training
        {
            Id = Guid.Parse("8388d571-d7cf-4ca8-8065-c2f7c1224e98"),
            Title = "Training 1",
            PhotoUrl = "photo1.jpg",
            Location = "Indoor",
            CoachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace",
            Coach = new Coach {Id="9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace", FirstName = "John", LastName= "Ivanov" },
            Duration = 1,
            TargetId = 1,
            Target = new Target { Name = "Endurance" },
            Price = 39.0m,
            JoinedClimbers = new List<TrainingClimber>(),
            isActive = true
        },
        new Training
        {
            Id = Guid.Parse("17cc72da-f2a2-4af8-bb67-3699db13fb1b"),
            Title = "Training 2",
            PhotoUrl = "photo2.jpg",
            Location = "Indoor",
            CoachId = "e81ccf9c-35aa-4adb-a23e-249e7096000e",
            Coach = new Coach {Id="e81ccf9c-35aa-4adb-a23e-249e7096000e", FirstName = "Maria" , LastName = "Ivanova" },
            Duration = 2,
            TargetId = 2,
            Target = new Target { Name = "Strength" },
            Price = 34.0m,
            JoinedClimbers = new List<TrainingClimber>
            {
                new TrainingClimber { ClimberId = "62764ec6-e266-4528-ae51-e44da8343b05" },
                new TrainingClimber { ClimberId = "3bf16936-2071-4762-b6aa-07524977acd6" }
            },
            isActive = true
        },
        new Training
        {
            Id = Guid.Parse("c329d216-6d21-4179-a9b0-a5a04b62c3ab"),
            Title = "Training 3",
            PhotoUrl = "photo3.jpg",
            Location = "indoor",
            CoachId = "325bcd07-bdb7-46eb-8050-1c1b79690aee",
            Coach = new Coach {Id="325bcd07-bdb7-46eb-8050-1c1b79690aee", FirstName = "Bob" , LastName="Peterson" },
            Duration = 3,
            TargetId = 1,
            Target = new Target { Name = "Power" },
            Price = 40.0m,
            JoinedClimbers = new List<TrainingClimber>
            {
                new TrainingClimber { ClimberId = "62764ec6-e266-4528-ae51-e44da8343b05" },
                new TrainingClimber { ClimberId = "24843754-8820-4953-9edb-88bf1c2ae23e" },
                new TrainingClimber { ClimberId = "a1f86467-c0fb-47d1-a5e2-69991e10c1e5" }
            },
            isActive = true
        },
        new Training
        {
            Id = Guid.Parse("d70cbeed-8aa8-4465-9bce-31c2ce73b455"),
            Title = "Training 4",
            PhotoUrl = "photo4.jpg",
            Location = "Location 4",
            CoachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace",
            Coach = new Coach {Id="9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace", FirstName = "John", LastName= "Ivanov" },
            Duration = 2,
            TargetId = 2,
            Target = new Target { Name = "Flexability" },
            Price = 20.0m,
            JoinedClimbers = new List<TrainingClimber>
            {
                new TrainingClimber { ClimberId = "3bf16936-2071-4762-b6aa-07524977acd6" },
                new TrainingClimber { ClimberId = "24843754-8820-4953-9edb-88bf1c2ae23e" }
            },
            isActive = true
        }
        };

            return trainingsData;
        }
    }
}
