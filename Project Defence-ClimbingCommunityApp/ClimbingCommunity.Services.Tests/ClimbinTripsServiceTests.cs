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
    using WebShopDemo.Core.Data.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using ClimbingCommunity.Services.Tests.ComparerViewModels;
    using ClimbingCommunity.Services.Tests.Mocking;
    using NUnit.Framework.Internal;

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
        public async Task Test_GetLastThreeClimbingTripsAsync()
        {
            // Arrange
            var tripData = TripsForTesting();


            var mockQueryable = new MockAsyncEnumerable<ClimbingTrip>(tripData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimbingTrip>(It.IsAny<Expression<Func<ClimbingTrip, bool>>>()))
                           .Returns(mockQueryable);

            var expectedViewModels = tripData
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
            CollectionAssert.AreEquivalent(expectedViewModels, result);
        }


        private static List<ClimbingTrip> TripsForTesting()
        {
            var tripData = new List<ClimbingTrip>
            {
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Trip 1",
                    PhotoUrl = "photo1.jpg",
                    Destination = "Destination 1",
                    OrganizatorId = "organizer1",
                    Duration = 5,
                    TripType = new TripType { Id = 1, Name = "Type 1" },
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-2)
                },
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Trip 2",
                    PhotoUrl = "photo2.jpg",
                    Destination = "Destination 2",
                    OrganizatorId = "organizer2",
                    Duration = 7,
                    TripType = new TripType { Id = 2, Name = "Type 2" },
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-3)
                },
                new ClimbingTrip
                {
                    Id = Guid.NewGuid(),
                    Title = "Trip 3",
                    PhotoUrl = "photo3.jpg",
                    Destination = "Destination 3",
                    OrganizatorId = "organizer3",
                    Duration = 3,
                    TripType = new TripType { Id = 3, Name = "Type 3" },
                    IsActive = false,
                    CreatedOn = DateTime.UtcNow.AddDays(-2)
                }
          };
            return tripData;
        }
    }
}
