namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Web.ViewModels.ClimbingTrip;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using ClimbingCommunity.Services.Tests.ComparerViewModels;
    using ClimbingCommunity.Services.Tests.Mocking;
    using NUnit.Framework.Internal;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Data.Common;

    [TestFixture]
    public class ClimbingTripServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IImageService> _imageServiceMock;
        private ClimbingTripService _climbingTripService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _imageServiceMock = new Mock<IImageService>();


            _climbingTripService = new ClimbingTripService(_repositoryMock.Object, _imageServiceMock.Object);
        }

        [Test]
        public async Task Test_CreateAsync_Success()
        {
            // Arrange
            var model = new ClimbingTripFormViewModel
            {
                Title = "Test Trip",
                Destination = "Test Destination",
                Duration = 5,
                TripTypeId = 1,
                PhotoFile = new Mock<IFormFile>().Object // Mock photo file
            };

            _imageServiceMock.Setup(service => service.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                             .ReturnsAsync("test-photo-url");

            // Act
            await _climbingTripService.CreateAsync("organizerId", model);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ClimbingTrip>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_CreateAsync_Failded_NullOrganizatorId_ReturnsModel()
        {
            // Arrange
            var model = new ClimbingTripFormViewModel
            {
                Title = "Test Trip",
                Destination = "Test Destination",
                Duration = 5,
                TripTypeId = 1,
                PhotoFile = new Mock<IFormFile>().Object
            };

            // Act and Assert

            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ClimbingTrip>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
            _imageServiceMock.Verify(service => service.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_CreateAsync_Failed_EmptyTitle_ReturnsModel()
        {
            // Arrange
            var model = new ClimbingTripFormViewModel
            {
                Title = "", // Empty Title
                Destination = "Test Destination",
                Duration = 5,
                TripTypeId = 1,
                PhotoFile = new Mock<IFormFile>().Object
            };


            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ClimbingTrip>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
            _imageServiceMock.Verify(service => service.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_CreateAsync_Failed_WithNullPhotoFile_ReturnsForm()
        {
            // Arrange
            var model = new ClimbingTripFormViewModel
            {
                Title = "Test Trip",
                Destination = "Test Destination",
                Duration = 5,
                TripTypeId = 1,
                PhotoFile = null // Set PhotoFile to null
            };

            // Act
            await _climbingTripService.CreateAsync("organizerId", model);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ClimbingTrip>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
            _imageServiceMock.Verify(service => service.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Test_CreateAsync_WithNullModel()
        {
            // Arrange and Act
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _climbingTripService.CreateAsync("organizerId", null));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_DeleteTripByIdAsync_Success()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(tripId), IsActive = true };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            await _climbingTripService.DeleteTripByIdAsync(tripId);

            // Assert
            Assert.IsFalse(climbingTrip.IsActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_DeleteTripByIdAsync_TripNotFound()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))!
                           .ReturnsAsync((ClimbingTrip)null!);

            // Act and Assert
            Assert.That(async () => await _climbingTripService.DeleteTripByIdAsync(tripId),
                        Throws.Exception, "Object reference not set to an instance of an object.");
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_DeleteTripByIdAsync_SaveChanges_Failure()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(tripId), IsActive = true };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(climbingTrip);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("SaveChanges failed"));

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () => await _climbingTripService.DeleteTripByIdAsync(tripId));

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_DeleteTripByIdAsync_AlreadyInactive()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(tripId), IsActive = false };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            await _climbingTripService.DeleteTripByIdAsync(tripId);

            // Assert
            Assert.IsFalse(climbingTrip.IsActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_DeleteTripByIdAsync_SaveChanges_Success()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(tripId), IsActive = true };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            await _climbingTripService.DeleteTripByIdAsync(tripId);

            // Assert
            Assert.IsFalse(climbingTrip.IsActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_EditClimbingTripByIdAsync_WithNullModel_Fail()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();

            // Act and Assert
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () =>
               await _climbingTripService.EditClimbingTripByIdAsync(tripId, null));

            Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));

            await Task.CompletedTask;

        }

        [Test]
        public async Task Test_EditClimbingTripByIdAsync_WithoutPhotoChange_Success()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var model = new ClimbingTripFormViewModel
            {
                Title = "Updated Trip",
                Duration = 6,
                Destination = "Updated Destination",
                PhotoFile = null
            };
            var existingTrip = new ClimbingTrip
            {
                Id = Guid.Parse(tripId),
                Title = "Original Trip",
                Duration = 5,
                Destination = "Original Destination",
                PhotoUrl = "existing-photo-url"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(existingTrip);

            // Act
            await _climbingTripService.EditClimbingTripByIdAsync(tripId, model);

            // Assert
            Assert.That(existingTrip.Title, Is.EqualTo(model.Title));
            Assert.That(existingTrip.Duration, Is.EqualTo(model.Duration));
            Assert.That(existingTrip.Destination, Is.EqualTo(model.Destination));
            Assert.That(existingTrip.PhotoUrl, Is.EqualTo("existing-photo-url"));
            _imageServiceMock.Verify(service => service.DeletePicture(It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_EditClimbingTripByIdAsync_WithPhotoChange_Success()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();

            var model = new ClimbingTripFormViewModel
            {
                Title = "Updated Trip",
                Duration = 6,
                Destination = "Updated Destination",
                PhotoFile = new Mock<IFormFile>().Object
            };
            var existingTrip = new ClimbingTrip
            {
                Id = Guid.Parse(tripId),
                Title = "Original Trip",
                Duration = 5,
                Destination = "Original Destination",
                PhotoUrl = "existing-photo-url"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(existingTrip);
            _imageServiceMock.Setup(service => service.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                             .ReturnsAsync("new-photo-url");

            // Act
            await _climbingTripService.EditClimbingTripByIdAsync(tripId, model);

            // Assert
            Assert.That(existingTrip.Title, Is.EqualTo(model.Title));
            Assert.That(existingTrip.Duration, Is.EqualTo(model.Duration));
            Assert.That(existingTrip.Destination, Is.EqualTo(model.Destination));
            Assert.That(existingTrip.PhotoUrl, Is.EqualTo("new-photo-url"));
            _imageServiceMock.Verify(service => service.DeletePicture("existing-photo-url"), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_EditClimbingTripByIdAsync_WithInvalidIdGuidFormat_Fail()
        {
            // Arrange
            var invalidTripId = "invalid-guid-format"; // Invalid GUID format
            var model = new ClimbingTripFormViewModel
            {
                Title = "Updated Trip",
                Duration = 6,
                Destination = "Updated Destination",
                PhotoFile = null
            };

            // Act and Assert
            var exception = Assert.ThrowsAsync<FormatException>(async () =>
                await _climbingTripService.EditClimbingTripByIdAsync(invalidTripId, model));
            Assert.That(exception.Message, Is.EqualTo("Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)."));

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_EditClimbingTripByIdAsync_SaveChangesFailure_Fail()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var model = new ClimbingTripFormViewModel
            {
                Title = "Updated Trip",
                Duration = 6,
                Destination = "Updated Destination",
                PhotoFile = null
            };
            var existingTrip = new ClimbingTrip
            {
                Id = Guid.Parse(tripId),
                Title = "Original Trip",
                Duration = 5,
                Destination = "Original Destination",
                PhotoUrl = "existing-photo-url"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(existingTrip);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("Object reference not set to an instance of an object."));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(async () =>
               await _climbingTripService.EditClimbingTripByIdAsync(tripId, model));
            Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));

            await Task.CompletedTask;

        }

        [Test]
        public async Task Test_GetAllTripTypesAsync()
        {
            // Arrange
            var tripTypesData = new List<TripType>
    {
        new TripType { Id = 1, Name = "Type 1" },
        new TripType { Id = 2, Name = "Type 2" },
        new TripType { Id = 3, Name = "Type 3" }
    };
            var mockQueryable = new MockAsyncEnumerable<TripType>(tripTypesData);

            _repositoryMock.Setup(repo => repo.AllReadonly<TripType>())
                           .Returns(mockQueryable);

            var expectedViewModels = tripTypesData.Select(tt => new TripTypeViewModel
            {
                Id = tt.Id,
                Name = tt.Name
            });

            // Act
            var result = await _climbingTripService.GetAllTripTypesAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new TripTypeViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripTypesAsync_EmptyList()
        {
            // Arrange
            var tripTypesData = new List<TripType>();
            var mockQueryable = new MockAsyncEnumerable<TripType>(tripTypesData);

            _repositoryMock.Setup(repo => repo.AllReadonly<TripType>())
                           .Returns(mockQueryable);

            // Act
            var result = await _climbingTripService.GetAllTripTypesAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllTripTypesAsync_NullRepositoryResult()
        {
            // Arrange
            var mockAsyncEnumerable = new MockAsyncEnumerable<TripType>(Enumerable.Empty<TripType>());

            _repositoryMock.Setup(repo => repo.AllReadonly<TripType>())
                           .Returns(mockAsyncEnumerable);

            // Act
            var result = await _climbingTripService.GetAllTripTypesAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllTripTypesAsync_NoTypes()
        {
            // Arrange
            var mockAsyncEnumerable = new MockAsyncEnumerable<TripType>(Enumerable.Empty<TripType>());

            _repositoryMock.Setup(repo => repo.AllReadonly<TripType>())
                           .Returns(mockAsyncEnumerable);

            // Act
            var result = await _climbingTripService.GetAllTripTypesAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetClimbingTripForEditAsync_Success()
        {
            // Arrange
            var tripId = "8CA27E64-630B-49FB-A62B-E6467434CD0A"; // Replace with a valid trip ID
            var trip = new ClimbingTrip
            {
                Id = Guid.Parse(tripId),
                Title = "Test Trip",
                PhotoUrl = "test-photo-url",
                Destination = "Test Destination",
                Duration = 5,
                TripTypeId = 1,
                OrganizatorId = "organizerId"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(trip);

            var expectedViewModel = new ClimbingTripFormViewModel()
            {
                Title = trip.Title,
                PhotoUrl = trip.PhotoUrl,
                Destination = trip.Destination,
                Duration = trip.Duration,
                TripTypeId = trip.TripTypeId,
                OrganizatorId = trip.OrganizatorId,
                IsEditModel = true
            };

            // Act
            var result = await _climbingTripService.GetClimbingTripForEditAsync(tripId);

            // Assert
            Assert.That(result.Title, Is.EqualTo(expectedViewModel.Title));
            Assert.That(result.PhotoUrl, Is.EqualTo(expectedViewModel.PhotoUrl));
            Assert.That(result.Destination, Is.EqualTo(expectedViewModel.Destination));
            Assert.That(result.Duration, Is.EqualTo(expectedViewModel.Duration));
            Assert.That(result.TripTypeId, Is.EqualTo(expectedViewModel.TripTypeId));
            Assert.That(result.OrganizatorId, Is.EqualTo(expectedViewModel.OrganizatorId));
            Assert.That(result.IsEditModel, Is.EqualTo(expectedViewModel.IsEditModel));
        }
        [Test]
        public async Task Test_GetClimbingTripForEditAsync_InvalidTripId()
        {
            // Arrange
            var invalidTripId = "8CA27E64-630B-49FB-A62B-E6467434CD12";
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(invalidTripId)))!
                           .ReturnsAsync((ClimbingTrip)null!);

            // Act 
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () =>
         await _climbingTripService.GetClimbingTripForEditAsync(invalidTripId));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));

            await Task.CompletedTask;

        }
        [Test]
        public async Task Test_IsClimbingTripExistsByIdAsync_ValidIdExists()
        {
            // Arrange
            var validTripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(validTripId)))
                           .ReturnsAsync(new ClimbingTrip());

            // Act
            var result = await _climbingTripService.IsClimbingTripExistsByIdAsync(validTripId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsClimbingTripExistsByIdAsync_ValidIdDoesNotExist()
        {
            // Arrange
            var validTripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(validTripId)))
                           .ReturnsAsync((ClimbingTrip)null!);

            // Act
            var result = await _climbingTripService.IsClimbingTripExistsByIdAsync(validTripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsClimbingTripExistsByIdAsync_InvalidGuidFormat()
        {
            // Arrange
            var invalidTripId = "invalid-guid";

            // Act
            var result = await _climbingTripService.IsClimbingTripExistsByIdAsync(invalidTripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsClimbingTripExistsByIdAsync_NullId()
        {
            // Act
            var result = await _climbingTripService.IsClimbingTripExistsByIdAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsClimbingTripExistsByIdAsync_EmptyId()
        {
            // Act
            var result = await _climbingTripService.IsClimbingTripExistsByIdAsync(string.Empty);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsTripTypeExistsByIdAsync_Exists()
        {
            // Arrange
            var tripTypeId = 1;
            _repositoryMock.Setup(repo => repo.GetByIdAsync<TripType>(tripTypeId))
                           .ReturnsAsync(new TripType { Id = tripTypeId });

            // Act
            var result = await _climbingTripService.IsTripTypeExistsByIdAsync(tripTypeId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsTripTypeExistsByIdAsync_NotExists()
        {
            // Arrange
            var tripTypeId = 1800;
            _repositoryMock.Setup(repo => repo.GetByIdAsync<TripType>(tripTypeId))
                           .ReturnsAsync((TripType)null!);

            // Act
            var result = await _climbingTripService.IsTripTypeExistsByIdAsync(tripTypeId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsTripTypeExistsByIdAsync_InvalidId()
        {
            // Arrange
            var invalidTripTypeId = -1;
            _repositoryMock.Setup(repo => repo.GetByIdAsync<TripType>(invalidTripTypeId))
                           .ReturnsAsync((TripType)null!);

            // Act
            var result = await _climbingTripService.IsTripTypeExistsByIdAsync(invalidTripTypeId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsTripTypeExistsByIdAsync_ZeroId()
        {
            // Arrange
            var tripTypeId = 0;
            _repositoryMock.Setup(repo => repo.GetByIdAsync<TripType>(tripTypeId))
                           .ReturnsAsync((TripType)null!);

            // Act
            var result = await _climbingTripService.IsTripTypeExistsByIdAsync(tripTypeId);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_IsUserOrganizatorOfTripByIdAsync_Organizator()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var trip = new ClimbingTrip { OrganizatorId = userId };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(trip);

            // Act
            var result = await _climbingTripService.isUserOrganizatorOfTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsUserOrganizatorOfTripByIdAsync_NotOrganizator()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var trip = new ClimbingTrip { OrganizatorId = "otherUser" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(trip);

            // Act
            var result = await _climbingTripService.isUserOrganizatorOfTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserOrganizatorOfTripByIdAsync_TripNotFound()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync((ClimbingTrip)null!);



            // Act , Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
        await _climbingTripService.isUserOrganizatorOfTripByIdAsync(userId, tripId), "Object reference not set to an instance of an object.");
        }

        [Test]
        public async Task Test_IsUserOrganizatorOfTripByIdAsync_UserNotFound()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var trip = new ClimbingTrip { OrganizatorId = userId };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(trip);

            // Act
            var result = await _climbingTripService.isUserOrganizatorOfTripByIdAsync("otherUser", tripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserOrganizatorOfTripByIdAsync_StringEmpty()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var trip = new ClimbingTrip { OrganizatorId = string.Empty };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(tripId)))
                           .ReturnsAsync(trip);

            // Act
            var result = await _climbingTripService.isUserOrganizatorOfTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_UserParticipating()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(new object[] { Guid.Parse(tripId), userId }))
                           .ReturnsAsync(tripClimber);

            // Act
            var result = await _climbingTripService.IsUserParticipateInTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_UserNotParticipating()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(new object[] { Guid.Parse(tripId), userId }))
                           .ReturnsAsync((TripClimber)null!);

            // Act
            var result = await _climbingTripService.IsUserParticipateInTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_InvalidUserId()
        {
            // Arrange
            var userId = "invalidUserId";
            var tripId = Guid.NewGuid().ToString();

            // Act
            var result = await _climbingTripService.IsUserParticipateInTripByIdAsync(userId, tripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_InvalidTripId()
        {
            // Arrange
            var userId = "user123";
            var invalidTripId = "8CA27E64-630B-49FB-A62B-E6467434CD12";

            // Act
            var result = await _climbingTripService.IsUserParticipateInTripByIdAsync(userId, invalidTripId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_EmptyStringsIds()
        {
            // Arrange
            string userId = string.Empty;
            string tripId = string.Empty;

            Assert.ThrowsAsync<FormatException>(async () =>
        await _climbingTripService.IsUserParticipateInTripByIdAsync(userId, tripId), "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_IsUserParticipateInTripByIdAsync_UserParticipatingWithInvalidTripId()
        {
            // Arrange
            var userId = "user123";
            var tripId = Guid.NewGuid().ToString();
            var invalidTripId = "invalidId";
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(new object[] { Guid.Parse(tripId), userId }))
                           .ReturnsAsync(tripClimber);

            Assert.ThrowsAsync<FormatException>(async () =>
          await _climbingTripService.isUserOrganizatorOfTripByIdAsync(userId, invalidTripId), "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_JoinClimbingTripAsync_SuccessfulJoin()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";

            // Act
            await _climbingTripService.JoinClimbingTripAsync(tripId, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync<TripClimber>(It.IsAny<TripClimber>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_JoinClimbingTripAsync_InvalidTripId()
        {
            // Arrange
            var invalidTripId = "invalidId";
            var userId = "user123";

            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
                await _climbingTripService.JoinClimbingTripAsync(invalidTripId, userId));
            await Task.CompletedTask;
        }


        [Test]
        public async Task Test_JoinClimbingTripAsync_DuplicateJoin()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.AddAsync<TripClimber>(It.IsAny<TripClimber>()))
                           .ThrowsAsync(new DbUpdateException("Duplicate entry"));

            // Act and Assert
            Assert.ThrowsAsync<DbUpdateException>(async () =>
               await _climbingTripService.JoinClimbingTripAsync(tripId, userId));
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_JoinClimbingTripAsync_SaveChangesFailure()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("SaveChanges failed"));

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () =>
               await _climbingTripService.JoinClimbingTripAsync(tripId, userId));
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_JoinClimbingTripAsync_StringEmptyTripId()
        {
            // Arrange
            string tripId = string.Empty;
            var userId = "user123";

            // Act and Assert

            Assert.ThrowsAsync<FormatException>(async () =>
          await _climbingTripService.isUserOrganizatorOfTripByIdAsync(tripId, userId), "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_LeaveClimbingTripByIdAsync_SuccessfulLeave()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync(tripClimber);

            // Act
            await _climbingTripService.LeaveClimbingTripByIdAsync(tripId, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync<TripClimber>(It.IsAny<TripClimber>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_LeaveClimbingTripByIdAsync_NotParticipating()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync((TripClimber)null!);

            // Act and Assert
            Assert.DoesNotThrowAsync(async () =>
               await _climbingTripService.LeaveClimbingTripByIdAsync(tripId, userId));

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_LeaveClimbingTripByIdAsync_InvalidTripId()
        {
            // Arrange
            var invalidTripId = "invalidId";
            var userId = "user123";

            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
               await _climbingTripService.LeaveClimbingTripByIdAsync(invalidTripId, userId));

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_LeaveClimbingTripByIdAsync_DeleteFailure()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync(tripClimber);
            _repositoryMock.Setup(repo => repo.DeleteAsync<TripClimber>(It.IsAny<TripClimber>()))
                           .ThrowsAsync(new Exception("Delete failed"));

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () =>
               await _climbingTripService.LeaveClimbingTripByIdAsync(tripId, userId));

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_LeaveClimbingTripByIdAsync_SaveChangesFailure()
        {
            // Arrange
            var tripId = Guid.NewGuid().ToString();
            var userId = "user123";
            var tripClimber = new TripClimber { ClimberId = userId, TripId = Guid.Parse(tripId) };
            _repositoryMock.Setup(repo => repo.GetByIdsAsync<TripClimber>(It.IsAny<object[]>()))
                           .ReturnsAsync(tripClimber);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("SaveChanges failed"));

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () =>
               await _climbingTripService.LeaveClimbingTripByIdAsync(tripId, userId));

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithTitleFilter()
        {
            // Arrange
            var searchText = "ing";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.Title.Contains(searchText))
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert

            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithTripTypeFilter()
        {
            // Arrange
            var searchText = "ing";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.TripType.Name.Contains(searchText))
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithDestinationFilter()
        {
            // Arrange
            var searchText = "land";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.Destination.Contains(searchText)) // Match destination
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithOrganizerFilter()
        {
            // Arrange
            var searchText = "an";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.Organizator.FirstName.Contains(searchText) || ct.Organizator.LastName.Contains(searchText)) // Match organizer's name
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    Organizator = ct.Organizator
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithNoMatchingFilters()
        {
            // Arrange
            var searchText = "not found filter";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(Enumerable.Empty<ClimbingTrip>());

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = Enumerable.Empty<ClimbingTripViewModel>();
            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithCombinedFilters()
        {
            // Arrange
            var searchText = "Finland"; // Match both Title and Destination
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.Title.Contains(searchText) || ct.Destination.Contains(searchText))
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithEmptySearchText()
        {
            // Arrange
            var searchText = string.Empty;
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(Enumerable.Empty<ClimbingTrip>());

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = Enumerable.Empty<ClimbingTripViewModel>();

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingByStringTripsAsync_WithCaseSensitiveSearchText()
        {
            // Arrange
            var searchText = "FiNLand";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            IEnumerable<ClimbingTripViewModel> expectedViewModels = tripsData
                .OrderByDescending(x => x.CreatedOn)
                .Where(ct => ct.Destination.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingByStringTripsAsync(searchText);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingTripsAsync_ReturnsAllActiveTrips()
        {
            // Arrange
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count()
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }


        [Test]
        public async Task Test_GetAllClimbingTripsAsync_NoActiveTrips_ReturnsEmpty()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData.ForEach(trip => trip.IsActive = false);

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(Enumerable.Empty<ClimbingTrip>());

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = Enumerable.Empty<ClimbingTripViewModel>();

            // Act
            var result = await _climbingTripService.GetAllClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllClimbingTripsAsync_WithParticipantsCount()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].Climbers.Add(new TripClimber());

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count()
                });

            // Act
            var result = await _climbingTripService.GetAllClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllClimbingTripsAsync_MixedActiveAndInactiveTrips()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].IsActive = false;
            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count()
                });

            // Act

            var result = await _climbingTripService.GetAllClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());


        }
        [Test]
        public async Task Test_GetAllClimbingTripsAsync_NoTrips_ReturnsEmpty()
        {
            // Arrange
            var tripsData = new List<ClimbingTrip>();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(Enumerable.Empty<ClimbingTrip>());

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = Enumerable.Empty<ClimbingTripViewModel>();

            // Act
            var result = await _climbingTripService.GetAllClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllJoinedClimbingTripsByUserIdAsync_UserJoinsMultipleActiveTrips()
        {
            // Arrange
            var userId = "a923b58b-aa08-4b8d-881c-29f001074473";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true && ct.Climbers.Any(c => c.ClimberId == userId)));

            var expectedViewModels = tripsData
                .Where(ct => (ct.IsActive == true || ct.IsActive == null) && ct.Climbers.Any(c => c.ClimberId == userId))
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new JoinedClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count(),
                });

            // Act
            var result = await _climbingTripService.GetAllJoinedClimbingTripsByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllJoinedClimbingTripsByUserIdAsync_InvalidUserIdJoinsNoTrips()
        {
            // Arrange
            var userId = "user1"; // Replace with a valid user ID
            var tripsData = TripsForTesting(); // User is not a climber in any trip

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.Climbers.Any(c => c.ClimberId == userId)));

            var expectedViewModels = Enumerable.Empty<JoinedClimbingTripViewModel>();

            // Act
            var result = await _climbingTripService.GetAllJoinedClimbingTripsByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllJoinedClimbingTripsByUserIdAsync_UserJoinsOnlyOneTrip()
        {
            // Arrange
            var userId = "3b699615-c307-4e09-ab9f-6f8c3538b248";
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true && ct.Climbers.Any(c => c.ClimberId == userId)));

            var expectedViewModels = tripsData
                .Where(ct => (ct.IsActive == true || ct.IsActive == null) && ct.Climbers.Any(c => c.ClimberId == userId))
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new JoinedClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count(),
                });

            // Act
            var result = await _climbingTripService.GetAllJoinedClimbingTripsByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllJoinedClimbingTripsByUserIdAsync_UserJoinsMultipleActiveAndUnActiveTrips()
        {
            // Arrange
            var userId = "a923b58b-aa08-4b8d-881c-29f001074473";
            var tripsData = TripsForTesting();
            tripsData[1].IsActive = false;

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true && ct.Climbers.Any(c => c.ClimberId == userId)));

            var expectedViewModels = tripsData
                .Where(ct => (ct.IsActive == true || ct.IsActive == null) && ct.Climbers.Any(c => c.ClimberId == userId))
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new JoinedClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    Organizator = ct.Organizator,
                    NumberOfParticipants = ct.Climbers.Count(),
                });

            // Act
            var result = await _climbingTripService.GetAllJoinedClimbingTripsByUserIdAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new JoinedClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripsForAdminAsync_NoTripsExist()
        {
            // Arrange
            var tripsData = new List<ClimbingTrip>(); // Empty list of trips

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>())
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = Enumerable.Empty<AdminActivityViewModel>();

            // Act
            var result = await _climbingTripService.GetAllTripsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripsForAdminAsync_MultipleTripsExist()
        {
            // Arrange
            var tripsData = TripsForTesting();

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>())
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn));

            var expectedViewModels = tripsData
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new AdminActivityViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = ct.IsActive,
                    Location = ct.Destination
                });

            // Act
            var result = await _climbingTripService.GetAllTripsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripsForAdminAsync_TripsWithMixedActiveAndInactiveStatus()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].IsActive = false; // Inactive trip

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>())
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct=>ct.IsActive==true));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true)
                .OrderByDescending(ct => ct.CreatedOn)
                .Select(ct => new AdminActivityViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = ct.IsActive,
                    Location = ct.Destination
                });

            // Act
            var result = await _climbingTripService.GetAllTripsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripsForAdminAsync_TripsWithInvalidDateCreatedOn()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].CreatedOn = DateTime.MinValue; // Trip with null CreatedOn date

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>())
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn)
                           .Where(ct=>ct.CreatedOn.Year>2022));

            var expectedViewModels = tripsData
                .OrderByDescending(ct => ct.CreatedOn)
                .Where(ct => ct.CreatedOn.Year > 2022)
                .Select(ct => new AdminActivityViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"), 
                    IsActive = ct.IsActive,
                    Location = ct.Destination
                });

            // Act
            var result = await _climbingTripService.GetAllTripsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllTripsForAdminAsync_TripsWithNullLocation()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].Destination = null; // Trip with null Destination

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>())
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn)
                           .Where(ct=>ct.Destination!=null));

            var expectedViewModels = tripsData
                .OrderByDescending(ct => ct.CreatedOn)
                .Where(ct => ct.Destination != null)
                .Select(ct => new AdminActivityViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    CreatedOn = ct.CreatedOn.ToString("yyyy-MM-dd"),
                    IsActive = ct.IsActive,
                    Location = ct.Destination
                });

            // Act
            var result = await _climbingTripService.GetAllTripsForAdminAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new AdminActivityViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeClimbingTripsAsync_BasicCaseWithActiveTrips()
        {
            // Arrange
            var tripsData = TripsForTesting();
            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Take(3)
                           .Where(ct=>ct.IsActive==true));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _climbingTripService.GetLastThreeClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeClimbingTripsAsync_ReturningOnlyTwoBecauseOtherInActive()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].IsActive = false;
            tripsData[1].IsActive = false;
            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);


            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _climbingTripService.GetLastThreeClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeClimbingTripsAsync_MultipleTripsWithSameCreatedOnDate()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].CreatedOn = DateTime.UtcNow;
            tripsData[1].CreatedOn = DateTime.UtcNow;

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn)
                           .Where(ct => ct.IsActive == true)
                           .Take(3));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _climbingTripService.GetLastThreeClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeClimbingTripsAsync_TripsWithDifferentTripTypes()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].TripType.Name = "Rope climbing";
            tripsData[1].TripType.Name = "Bouldering";
            tripsData[2].TripType.Name = "Sport climbing";

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn)
                           .Where(ct=>ct.IsActive==true)
                           .Take(3));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _climbingTripService.GetLastThreeClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }
        [Test]
        public async Task Test_GetLastThreeClimbingTripsAsync_ReturnsOldestBecauseMostRecentIsInActive()
        {
            // Arrange
            var tripsData = TripsForTesting();
            tripsData[0].IsActive = false;

            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable.OrderByDescending(x => x.CreatedOn)
                           .Where(ct=>ct.IsActive==true)
                           .Take(3));

            var expectedViewModels = tripsData
                .Where(ct => ct.IsActive == true || ct.IsActive == null)
                .OrderByDescending(ct => ct.CreatedOn)
                .Take(3)
                .Select(ct => new ClimbingTripViewModel
                {
                    Id = ct.Id.ToString(),
                    Title = ct.Title,
                    PhotoUrl = ct.PhotoUrl,
                    Destination = ct.Destination,
                    OrganizatorId = ct.OrganizatorId,
                    Duration = ct.Duration,
                    TripType = ct.TripType.Name,
                    isOrganizator = false
                });

            // Act
            var result = await _climbingTripService.GetLastThreeClimbingTripsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimbingTripViewModelComparer());
        }



        private static List<ClimbingTrip> TripsForTesting()
        {
            var tripData = new List<ClimbingTrip>
            {
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Finland solo climbing",
                    PhotoUrl = "photo1.jpg",
                    Destination = "FiNland",
                    OrganizatorId = "organizer1",
                    Duration = 5,
                    CreatedOn = DateTime.UtcNow,
                    TripType = new TripType()
                    {
                        Id = 1,
                        Name = "Rope climbing"
                    },
                    Organizator = new Climber()
                    {
                        FirstName = "Stoyan"
                    },
                    IsActive = true,
                    TripTypeId = 1,
                    Climbers = {new TripClimber()
                    {
                        ClimberId = "a923b58b-aa08-4b8d-881c-29f001074473"
                    },
                       new TripClimber()
                       {
                           ClimberId="3b699615-c307-4e09-ab9f-6f8c3538b248"
                       }
                    }
                },
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Finland rope climbing",
                    PhotoUrl = "photo2.jpg",
                    Destination = "Finland",
                    OrganizatorId = "organizer2",
                    Duration = 7,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    TripType = new TripType()
                    {
                        Id = 2,
                        Name = "Bouldering"
                    },
                    Organizator = new Climber()
                    {
                        FirstName = "Hristyan"
                    },
                    IsActive = true,
                    TripTypeId = 2,
                    Climbers = {new TripClimber()
                    {
                        ClimberId = "a923b58b-aa08-4b8d-881c-29f001074473"
                    } }
                },
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Finland bouldering",
                    PhotoUrl = "photo3.jpg",
                    Destination = "Finland",
                    OrganizatorId = "organizer3",
                    Duration = 3,
                    CreatedOn = DateTime.UtcNow.AddDays(-3),
                    TripType = new TripType()
                    {
                        Id = 3,
                        Name = "Bouldering"
                    },
                    Organizator = new Climber()
                    {
                        FirstName = "Ivan"
                    },
                    IsActive = true,
                    TripTypeId= 3,
                    Climbers = {new TripClimber()
                    {
                        ClimberId = "a923b58b-aa08-4b8d-881c-29f001074473"
                    } }
                },
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Finland solo climbing",
                    PhotoUrl = "photo1.jpg",
                    Destination = "FiNland",
                    OrganizatorId = "organizer1",
                    Duration = 5,
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                    TripType = new TripType()
                    {
                        Id = 1,
                        Name = "Rope climbing"
                    },
                    Organizator = new Climber()
                    {
                        FirstName = "Stoyan"
                    },
                    IsActive = true,
                    TripTypeId = 1,
                    Climbers = {
                        new TripClimber()
                        {
                            ClimberId = "a923b58b-aa08-4b8d-881c-29f001074473"
                        }
                    }
                }
            };
            return tripData;
        }
    }
}
