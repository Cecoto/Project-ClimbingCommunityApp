namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Data.Models.Enums;
    using ClimbingCommunity.Services.Contracts;
    using ClimbingCommunity.Services.Tests.ComparerViewModels;
    using ClimbingCommunity.Services.Tests.Mocking;
    using ClimbingCommunity.Web.ViewModels.AdminArea;
    using ClimbingCommunity.Web.ViewModels.Profile;
    using ClimbingCommunity.Web.ViewModels.User.Climber;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IRepository> _repositoryMock;
        private Mock<IImageService> _imageServiceMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRepository>();
            _imageServiceMock = new Mock<IImageService>();
            _userService = new UserService(_repositoryMock.Object, _imageServiceMock.Object);
        }
        [Test]
        public async Task Test_GetAllUsersAsync_ReturnsUsers()
        {
            // Arrange
            var usersData = UsersForTests();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(usersData);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedViewModels = usersData
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Age = user.Age,
                    Role = user.UserType
                });

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new UserViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllUsersAsync_NoUsers()
        {
            // Arrange
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(new List<ApplicationUser>());

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllUsersAsync_SingleUser()
        {
            // Arrange
            var usersData = UsersForTests().Take(1).ToList();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(usersData);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedViewModels = usersData
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Age = user.Age,
                    Role = user.UserType
                });

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new UserViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllUsersAsync_MultipleUsers()
        {
            // Arrange
            var usersData = UsersForTests();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(usersData);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedViewModels = usersData
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Age = user.Age,
                    Role = user.UserType
                });

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new UserViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllUsersAsync_MixedUsers()
        {
            // Arrange
            var mixedUsers = UsersForTests();
            mixedUsers[0].UserType = RoleConstants.Administrator;
            mixedUsers[1].UserType = RoleConstants.Coach;
            mixedUsers[2].UserType = RoleConstants.Climber;

            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(mixedUsers);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedViewModels = mixedUsers
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Age = user.Age,
                    Role = user.UserType
                });

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new UserViewModelComparer());
        }
        [Test]
        public async Task Test_GetAllUsersEmailsAsync_NoUsers()
        {
            // Arrange
            var emptyUsersList = new List<ApplicationUser>();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(emptyUsersList);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetAllUsersEmailsAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetAllUsersEmailsAsync_SingleUser()
        {
            // Arrange
            var usersData = UsersForTests().Take(1).ToList();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(usersData);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedEmails = usersData.Select(user => user.Email);

            // Act
            var result = await _userService.GetAllUsersEmailsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedEmails, result);
        }

        [Test]
        public async Task Test_GetAllUsersEmailsAsync_MultipleUsers()
        {
            // Arrange
            var usersData = UsersForTests();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(usersData);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedEmails = usersData.Select(user => user.Email);

            // Act
            var result = await _userService.GetAllUsersEmailsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedEmails, result);
        }

        [Test]
        public async Task Test_GetAllUsersEmailsAsync_OnlyAdmins()
        {
            // Arrange
            var adminUsers = UsersForTests().Where(u => u.UserType == RoleConstants.Administrator).ToList();
            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(adminUsers);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedEmails = adminUsers.Select(user => user.Email);

            // Act
            var result = await _userService.GetAllUsersEmailsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedEmails, result);
        }

        [Test]
        public async Task Test_GetAllUsersEmailsAsync_MixedUsers()
        {
            // Arrange
            var mixedUsers = UsersForTests();
            mixedUsers[0].UserType = RoleConstants.Administrator;
            mixedUsers[1].UserType = RoleConstants.Coach;
            mixedUsers[2].UserType = RoleConstants.Climber;

            var mockQueryable = new MockAsyncEnumerable<ApplicationUser>(mixedUsers);

            _repositoryMock.Setup(repo => repo.All<ApplicationUser>())
                           .Returns(mockQueryable);

            var expectedEmails = mixedUsers.Select(user => user.Email);

            // Act
            var result = await _userService.GetAllUsersEmailsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedEmails, result);
        }
        [Test]
        public async Task Test_GetClimberSpecialitiesForFormAsync()
        {
            // Arrange
            var specialitiesData = SpecialitiesForTests();

            var mockQueryable = new MockAsyncEnumerable<ClimberSpeciality>(specialitiesData);

            _repositoryMock.Setup(repo =>
                repo.AllReadonly<ClimberSpeciality>()
            ).Returns(mockQueryable);

            var expectedViewModels = specialitiesData
                .Select(cs => new ClimberSpecialityViewModel()
                {
                    Id = cs.Id,
                    Name = cs.Name,
                });

            // Act
            var result = await _userService.GetClimberSpecialitiesForFormAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimberSpecialityViewModelComparer());
        }
        [Test]
        public async Task Test_GetClimberSpecialitiesForFormAsync_ReturnsSpecialities()
        {
            // Arrange
            var specialitiesData = SpecialitiesForTests();
            var mockQueryable = new MockAsyncEnumerable<ClimberSpeciality>(specialitiesData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimberSpeciality>())
                           .Returns(mockQueryable);

            var expectedSpecialities = specialitiesData
                .Select(cs => new ClimberSpecialityViewModel
                {
                    Id = cs.Id,
                    Name = cs.Name
                });

            // Act
            var result = await _userService.GetClimberSpecialitiesForFormAsync();

            // Assert
            CollectionAssert.AreEqual(expectedSpecialities, result, new ClimberSpecialityViewModelComparer());
        }

        [Test]
        public async Task Test_GetClimberSpecialitiesForFormAsync_NoSpecialities()
        {
            // Arrange

            var data = new List<ClimberSpeciality>();
            var mockQueayable = new MockAsyncEnumerable<ClimberSpeciality>(data);
            _repositoryMock.Setup(repo => repo.AllReadonly<ClimberSpeciality>())
                           .Returns(mockQueayable);

            // Act
            var result = await _userService.GetClimberSpecialitiesForFormAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetClimberSpecialitiesForFormAsync_NullSpecialities()
        {
            var data = new List<ClimberSpeciality>();

            var mockQueayable = new MockAsyncEnumerable<ClimberSpeciality>(data);
            // Arrange
            _repositoryMock.Setup(repo => repo.AllReadonly<ClimberSpeciality>())
                     .Returns(mockQueayable);

            // Act & Assert

            await _userService.GetClimberSpecialitiesForFormAsync();

        }

        [Test]
        public async Task Test_GetClimberSpecialitiesForFormAsync_MultipleSpecialities()
        {
            // Arrange
            var specialitiesData = SpecialitiesForTests();
            var mockQueryable = new MockAsyncEnumerable<ClimberSpeciality>(specialitiesData);

            _repositoryMock.Setup(repo => repo.AllReadonly<ClimberSpeciality>())
                           .Returns(mockQueryable);

            var expectedSpecialities = specialitiesData
                .Select(cs => new ClimberSpecialityViewModel
                {
                    Id = cs.Id,
                    Name = cs.Name
                });

            // Act
            var result = await _userService.GetClimberSpecialitiesForFormAsync();

            // Assert
            CollectionAssert.AreEqual(expectedSpecialities, result, new ClimberSpecialityViewModelComparer());
        }
        [Test]
        public async Task Test_GetCoachInfoAsync_ValidCoachId()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            var expectedViewModel = new CoachProfileViewModel()
            {
                FirstName = coachData.FirstName,
                LastName = coachData.LastName,
                PhoneNumber = coachData.PhoneNumber,
                ProfilePicture = coachData.ProfilePictureUrl,
                Gender = coachData.Gender.ToString(),
                Id = coachData.Id.ToString(),
                CoachingExperience = coachData.CoachingExperience,
                TypeOfUser = coachData.UserType,
                Age = coachData.Age
            };

            // Act
            var result = await _userService.GetCoachInfoAsync(coachId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new CoachProfileViewModelComparer()));
        }
        [Test]
        public async Task Test_GetCoachInfoAsync_InvalidCoachId()
        {
            // Arrange
            var coachId = "invalidId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync((Coach)null!);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoAsync(coachId);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_GetCoachInfoAsync_NullProfilePicture()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);
            coachData.ProfilePictureUrl = null;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            var expectedViewModel = new CoachProfileViewModel()
            {
                FirstName = coachData.FirstName,
                LastName = coachData.LastName,
                PhoneNumber = coachData.PhoneNumber,
                ProfilePicture = null,
                Gender = coachData.Gender.ToString(),
                Id = coachData.Id.ToString(),
                CoachingExperience = coachData.CoachingExperience,
                TypeOfUser = coachData.UserType,
                Age = coachData.Age
            };

            // Act
            var result = await _userService.GetCoachInfoAsync(coachId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new CoachProfileViewModelComparer()));
        }

        [Test]
        public async Task Test_GetCoachInfoAsync_MissingCoach()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync((Coach)null!);

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoAsync(coachId);

            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_GetCoachInfoAsync_NullResult()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3asd";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoAsync(coachId);

            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetCoachInfoForUpdateAsync_ValidCoachId()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            var expectedViewModel = new UpdateCoachProfileViewModel()
            {
                ProfilePictureUrl = coachData.ProfilePictureUrl!,
                FirstName = coachData.FirstName,
                LastName = coachData.LastName,
                PhoneNumber = coachData.PhoneNumber,
                Age = coachData.Age,
                CoachingExperience = coachData.CoachingExperience
            };

            // Act
            var result = await _userService.GetCoachInfoForUpdateAsync(coachId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new UpdateCoachProfileViewModelComparer()));
        }

        [Test]
        public async Task Test_GetCoachInfoForUpdateAsync_InvalidCoachId()
        {
            // Arrange
            var coachId = "invalidId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync((Coach)null!);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoForUpdateAsync(coachId);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_GetCoachInfoForUpdateAsync_NullProfilePicture()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);
            coachData.ProfilePictureUrl = null;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            var expectedViewModel = new UpdateCoachProfileViewModel()
            {
                ProfilePictureUrl = null, // Since ProfilePictureUrl is null
                FirstName = coachData.FirstName,
                LastName = coachData.LastName,
                PhoneNumber = coachData.PhoneNumber,
                Age = coachData.Age,
                CoachingExperience = coachData.CoachingExperience
            };

            // Act
            var result = await _userService.GetCoachInfoForUpdateAsync(coachId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedViewModel).Using(new UpdateCoachProfileViewModelComparer()));
        }

        [Test]
        public async Task Test_GetCoachInfoForUpdateAsync_MissingCoach()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync((Coach)null);

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoForUpdateAsync(coachId);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_GetCoachInfoForUpdateAsync_NullResult()
        {
            // Arrange
            var coachId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3123";
            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == coachId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(coachId))
                           .ReturnsAsync(coachData!);

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.GetCoachInfoForUpdateAsync(coachId);
            }, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetLevelsForFormAsync_ValidData()
        {
            // Arrange
            var levelsData = LevelsForTests();
            var mockQueryable = new MockAsyncEnumerable<Level>(levelsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Level>())
                           .Returns(mockQueryable);

            var expectedViewModels = levelsData
                .Select(level => new ClimberLevelViewModel
                {
                    Id = level.Id,
                    Name = level.Name
                });

            // Act
            var result = await _userService.GetLevelsForFormAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimberLevelViewModelComparer());
        }

        [Test]
        public async Task Test_GetLevelsForFormAsync_EmptyData()
        {
            // Arrange
            var mockQueryable = new MockAsyncEnumerable<Level>(new List<Level>());

            _repositoryMock.Setup(repo => repo.AllReadonly<Level>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetLevelsForFormAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetLevelsForFormAsync_NullData()
        {
            // Arrange
            var levels = new List<Level>();
            var mockQueryable = new MockAsyncEnumerable<Level>(levels);
            _repositoryMock.Setup(repo => repo.AllReadonly<Level>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetLevelsForFormAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetLevelsForFormAsync_MixedData()
        {
            // Arrange
            var levelsData = LevelsForTests();
            var mockQueryable = new MockAsyncEnumerable<Level>(levelsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Level>())
                           .Returns(mockQueryable);

            var expectedViewModels = levelsData
                .Select(level => new ClimberLevelViewModel
                {
                    Id = level.Id,
                    Name = level.Name
                });

            // Act
            var result = await _userService.GetLevelsForFormAsync();

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new ClimberLevelViewModelComparer());
        }

        [Test]
        public async Task Test_GetLevelsForFormAsync_InvalidData()
        {
            // Arrange
            var levelsData = LevelsForTests();
            var mockQueryable = new MockAsyncEnumerable<Level>(levelsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Level>())
                           .Returns(mockQueryable);

            var expectedViewModels = levelsData
                .Select(level => new ClimberLevelViewModel
                {
                    Id = level.Id + 1, // Invalid ID
                    Name = level.Name
                });

            // Act
            var result = await _userService.GetLevelsForFormAsync();

            // Assert
            CollectionAssert.AreNotEqual(expectedViewModels, result, new ClimberLevelViewModelComparer());
        }
        [Test]
        public async Task Test_GetPhotosForUserAsync_ValidUserId()
        {
            // Arrange
            var userId = "d7a6b4fd-f4f3-41a3-806a-82935ac66ea0";
            var photosData = PhotosForTests().Where(p => p.UserId == userId).ToList();

            var mockQueryable = new MockAsyncEnumerable<Photo>(photosData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Photo>())
                           .Returns(mockQueryable);

            var expectedViewModels = photosData
                .Select(p => new PhotoViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            // Act
            var result = await _userService.GetPhotosForUserAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new PhotoViewModelComparer());
        }

        [Test]
        public async Task Test_GetPhotosForUserAsync_EmptyResult()
        {
            // Arrange
            var userId = "d7a6b4fd-f4f3-41a3-806a-82935ac66e123";
            var photosData = PhotosForTests().Where(p => p.UserId == userId).ToList();

            var mockQueryable = new MockAsyncEnumerable<Photo>(photosData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Photo>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetPhotosForUserAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Test_GetPhotosForUserAsync_MultiplePhotosForUser()
        {
            // Arrange
            var userId = "multiplePhotosUserId";
            var photosData = PhotosForTests().Where(p => p.UserId == userId).ToList();

            var mockQueryable = new MockAsyncEnumerable<Photo>(photosData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Photo>())
                           .Returns(mockQueryable);

            var expectedViewModels = photosData
                .Select(p => new PhotoViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            // Act
            var result = await _userService.GetPhotosForUserAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new PhotoViewModelComparer());
        }
        [Test]
        public async Task Test_GetPhotosForUserAsync_NullUserId()
        {
            // Arrange
            var userId = (string)null;

            var mockQueryable = new MockAsyncEnumerable<Photo>(PhotosForTests());

            _repositoryMock.Setup(repo => repo.AllReadonly<Photo>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetPhotosForUserAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetPhotosForUserAsync_InvalidUserId()
        {
            // Arrange
            var userId = "9e72b708-3abf-4ff6-9988-204f09dd960e";

            var mockQueryable = new MockAsyncEnumerable<Photo>(PhotosForTests());

            _repositoryMock.Setup(repo => repo.AllReadonly<Photo>())
                           .Returns(mockQueryable);

            // Act
            var result = await _userService.GetPhotosForUserAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetUserByIdAsync_ValidUserId()
        {
            // Arrange
            var userId = "d7a6b4fd-f4f3-41a3-806a-82935ac66ea0";
            var user = new ApplicationUser { Id = userId };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public async Task Test_GetUserByIdAsync_InvalidUserId()
        {
            // Arrange
            var userId = "d7a6b4fd-f4f3-41a3-806a-82935ac66e12";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync((ApplicationUser)null!);

             await _userService.GetUserByIdAsync(userId);
            
        }
        [Test]
        public async Task Test_GetUserByIdAsync_NullUserId()
        {
            // Arrange & Act
            var result = await _userService.GetUserByIdAsync(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_GetUserByIdAsync_NonexistentUser()
        {
            // Arrange
            var userId = "nonexistentUserId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task Test_IsClimbingSpecialityIdValidByIdAsync_ValidId()
        {
            // Arrange
            var climberSpecialityId = 1;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimberSpeciality>(climberSpecialityId))
                           .ReturnsAsync(new ClimberSpeciality());

            // Act
            var result = await _userService.IsClimbingSpecialityIdValidByIdAsync(climberSpecialityId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsClimbingSpecialityIdValidByIdAsync_InvalidId()
        {
            // Arrange
            var climberSpecialityId = 10;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimberSpeciality>(climberSpecialityId))
                           .ReturnsAsync((ClimberSpeciality)null!);

            // Act
            var result = await _userService.IsClimbingSpecialityIdValidByIdAsync(climberSpecialityId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsClimbingSpecialityIdValidByIdAsync_NegativeId()
        {
            // Arrange & Act
            var result = await _userService.IsClimbingSpecialityIdValidByIdAsync(-1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsClimbingSpecialityIdValidByIdAsync_ZeroId()
        {
            // Arrange & Act
            var result = await _userService.IsClimbingSpecialityIdValidByIdAsync(0);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_IsLevelIdValidByIdAsync_ValidId()
        {
            // Arrange
            var levelId = 1;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Level>(levelId))
                           .ReturnsAsync(new Level());

            // Act
            var result = await _userService.IsLevelIdValidByIdAsync(levelId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsLevelIdValidByIdAsync_InvalidId()
        {
            // Arrange
            var levelId = 10;

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Level>(levelId))
                           .ReturnsAsync((Level)null!);

            // Act
            var result = await _userService.IsLevelIdValidByIdAsync(levelId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsLevelIdValidByIdAsync_NegativeId()
        {
            // Arrange & Act
            var result = await _userService.IsLevelIdValidByIdAsync(-1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsLevelIdValidByIdAsync_ZeroId()
        {
            // Arrange & Act
            var result = await _userService.IsLevelIdValidByIdAsync(0);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_IsUserExistsByIdAsync_ValidUserId()
        {
            // Arrange
            var userId = "validUserId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync(new ApplicationUser());

            // Act
            var result = await _userService.IsUserExistsByIdAsync(userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsUserExistsByIdAsync_InvalidUserId()
        {
            // Arrange
            var userId = "invalidUserId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _userService.IsUserExistsByIdAsync(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserExistsByIdAsync_NullUserId()
        {
            // Arrange & Act
            var result = await _userService.IsUserExistsByIdAsync(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsUserExistsByIdAsync_EmptyUserId()
        {
            // Arrange & Act
            var result = await _userService.IsUserExistsByIdAsync("");

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_SavePhotosToUserByIdAsync_ValidData()
        {
            // Arrange
            var userId = "validUserId";
            var savedPhotoPaths = new List<string> { "photo1.jpg", "photo2.jpg", "photo3.jpg" };

            _repositoryMock.Setup(repo => repo.AddRangeAsync<Photo>(It.IsAny<IEnumerable<Photo>>()))
                           .Callback<IEnumerable<Photo>>(photos => { });

            // Act
            await _userService.SavePhotosToUserByIdAsync(userId, savedPhotoPaths);

            // Assert
            _repositoryMock.Verify(repo => repo.AddRangeAsync<Photo>(It.IsAny<IEnumerable<Photo>>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_SavePhotosToUserByIdAsync_EmptyPhotoPaths()
        {
            // Arrange
            var userId = "validUserId";
            var savedPhotoPaths = new List<string>();

            // Act
            await _userService.SavePhotosToUserByIdAsync(userId, savedPhotoPaths);

        }

        [Test]
        public async Task Test_SavePhotosToUserByIdAsync_NullUserId()
        {
            // Arrange
            var savedPhotoPaths = new List<string> { "photo1.jpg", "photo2.jpg" };

            // Act & Assert
           
                await _userService.SavePhotosToUserByIdAsync(null, savedPhotoPaths);
           
        }

        [Test]
        public async Task Test_SavePhotosToUserByIdAsync_NullPhotoPaths()
        {
            // Arrange
            var userId = "validUserId";

            // Act & Assert
             Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userService.SavePhotosToUserByIdAsync(userId, null);
            });

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_UpdateCoachInfoAsync_UpdateProfileWithPhoto()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var model = new UpdateCoachProfileViewModel
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "12345678909",
                Age = 30,
                CoachingExperience = 2,
                PhotoFile = new Mock<IFormFile>().Object
            };

            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == userId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(userId))
                           .ReturnsAsync(coachData);

            _imageServiceMock.Setup(imageService => imageService.SavePictureAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                             .ReturnsAsync("newPhotoUrl");

            // Act
            await _userService.UpdateCoachInfoAsync(userId, model);

            // Assert
            Assert.That(coachData.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(coachData.LastName, Is.EqualTo(model.LastName));
            Assert.That(coachData.PhoneNumber, Is.EqualTo(model.PhoneNumber));
            Assert.That(coachData.ProfilePictureUrl, Is.EqualTo("newPhotoUrl"));
            Assert.That(coachData.Age, Is.EqualTo(model.Age));
            Assert.That(coachData.CoachingExperience, Is.EqualTo(model.CoachingExperience));
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_UpdateCoachInfoAsync_UpdateProfileWithoutPhoto()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var model = new UpdateCoachProfileViewModel
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                Age = 30,
                CoachingExperience = 15,
                PhotoFile = null 
            };

            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == userId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(userId))!
                           .ReturnsAsync(coachData);

            // Act
            await _userService.UpdateCoachInfoAsync(userId, model);

            // Assert
            Assert.That(coachData.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(coachData.LastName, Is.EqualTo(model.LastName));
            Assert.That(coachData.PhoneNumber, Is.EqualTo(model.PhoneNumber));
            Assert.That(coachData.ProfilePictureUrl, Is.EqualTo("profile.jpg")); 
            Assert.That(coachData.Age, Is.EqualTo(model.Age));
            Assert.That(coachData.CoachingExperience, Is.EqualTo(model.CoachingExperience));
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_UpdateCoachInfoAsync_UpdateProfileWithNullPhoto()
        {
            // Arrange
            var userId = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace";
            var model = new UpdateCoachProfileViewModel
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                Age = 30,
                CoachingExperience =11,
                PhotoFile = null
            };

            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == userId);
            coachData.ProfilePictureUrl = "oldPhotoUrl";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(userId))
                           .ReturnsAsync(coachData);

            // Act
            await _userService.UpdateCoachInfoAsync(userId, model);

            // Assert
            Assert.That(coachData.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(coachData.LastName, Is.EqualTo(model.LastName));
            Assert.That(coachData.PhoneNumber, Is.EqualTo(model.PhoneNumber));
            Assert.That(coachData.ProfilePictureUrl, Is.EqualTo("oldPhotoUrl"));
            Assert.That(coachData.Age, Is.EqualTo(model.Age));
            Assert.That(coachData.CoachingExperience, Is.EqualTo(model.CoachingExperience));
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_UpdateCoachInfoAsync_InvalidUserId()
        {
            // Arrange
            var userId = "invalidUserId"; // Invalid user ID
            var model = new UpdateCoachProfileViewModel
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                Age = 30,
                CoachingExperience = 3,
                PhotoFile = null
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(userId))
                           .ReturnsAsync((Coach)null!);

            // Act & Assert
             Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.UpdateCoachInfoAsync(userId, model);
            }, "Object reference not set to an instance of an object.");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_UpdateCoachInfoAsync_InvalidModel()
        {
            // Arrange
            var userId = "validUserId";
            var model = new UpdateCoachProfileViewModel
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                Age = 30,
                CoachingExperience = 6,
                PhotoFile = null
            };

            var coachData = CoachesForTests().FirstOrDefault(c => c.Id == userId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Coach>(userId))!
                           .ReturnsAsync(coachData);

            // Act & Assert
             Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _userService.UpdateCoachInfoAsync(userId, null!); // Invalid model
            }, "Object reference not set to an instance of an object.");

            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
            await Task.CompletedTask;
        }


        private List<Photo> PhotosForTests()
        {
            var photosData = new List<Photo>
    {
        new Photo
        {
            Id = 1,
            UserId = "d7a6b4fd-f4f3-41a3-806a-82935ac66ea0",
            ImageUrl = "photo1.jpg"
        },
        new Photo
        {
            Id = 2,
            UserId = "d698c872-adb8-47b6-b914-ac1e269adbc6",
            ImageUrl = "photo2.jpg"
        },
        new Photo
        {
            Id = 3,
            UserId = "46da5009-eacf-42db-81a5-252fe19aed4e",
            ImageUrl = "photo3.jpg"
        }
    };

            return photosData;
        }

        private List<Level> LevelsForTests()
        {
            var levelsData = new List<Level>
    {
        new Level
        {
            Id = 1,
            Name = "Beginner"
        },
        new Level
        {
            Id = 2,
            Name = "Intermediate"
        },
        new Level
        {
            Id = 3,
            Name = "Advanced"
        }
    };

            return levelsData;
        }

        private List<Coach> CoachesForTests()
        {
            var coachesData = new List<Coach>
    {
        new Coach
        {
            Id = "9ffbd1ab-f57a-462d-b4ac-8d942e2c3ace",
            FirstName = "Pesho",
            LastName = "Petrov",
            PhoneNumber = "12345678910",
            ProfilePictureUrl = "profile.jpg",
            Gender = Gender.Male,
            CoachingExperience = 3,
            UserType = RoleConstants.Coach,
            Age = 30
        },
        new Coach
        {
            Id = "a8e15cb8-3dae-40ce-87cf-fdf0c98faafb",
            FirstName = "Ivan",
            LastName = "Ivanonv",
            PhoneNumber = "12345678910",
            ProfilePictureUrl = "profile.jpg",
            Gender = Gender.Male,
            CoachingExperience = 3,
            UserType = RoleConstants.Coach,
            Age = 32
        },
        new Coach
        {
            Id = "fc0e5b69-0f2f-44de-9bd6-6bc6b23a5a42",
            FirstName = "Ico",
            LastName = "Icov",
            PhoneNumber = "123456789",
            ProfilePictureUrl = "profile.jpg",
            Gender = Gender.Male,
            CoachingExperience = 3,
            UserType = RoleConstants.Coach,
            Age = 24
        },
    };

            return coachesData;
        }
        private List<ClimberSpeciality> SpecialitiesForTests()
        {
            var specialitiesData = new List<ClimberSpeciality>
    {
        new ClimberSpeciality
        {
            Id = 1,
            Name = "Bouldering"
        },
        new ClimberSpeciality
        {
            Id = 2,
            Name = "Sport Climbing"
        },
        new ClimberSpeciality
        {
            Id = 3,
            Name = "Trad Climbing"
        }
    };

            return specialitiesData;
        }

        private List<Climber> ClimbersForTests()
        {
            var climberData = new List<Climber>
    {
        new Climber
        {
            Id = "24843754-8820-4953-9edb-88bf1c2ae23e",
            FirstName = "Maria",
            LastName = "Ivanonva",
            PhoneNumber = "12345678910",
            ProfilePictureUrl = "profile1.jpg",
            Gender = Gender.Female,
            ClimberSpecialityId = 1,
            ClimberSpeciality = new ClimberSpeciality { Name = "Bouldering" },
            ClimbingExperience = 2,
            UserType = RoleConstants.Climber,
            LevelId = 1,
            Level = new Level { Name = "Beginner" },
            Age = 25
        },
        new Climber
        {
            Id = "a1f86467-c0fb-47d1-a5e2-69991e10c1e5",
            FirstName = "Ivan",
            LastName = "Ivanonv",
            PhoneNumber = "10987654321",
            ProfilePictureUrl = "profile2.jpg",
            Gender = Gender.Male,
            ClimberSpecialityId = 2,
            ClimberSpeciality = new ClimberSpeciality { Name = "Lead Climbing" },
            ClimbingExperience = 7,
            UserType = RoleConstants.Climber,
            LevelId = 2,
            Level = new Level { Name = "Intermediate" },
            Age = 30
        }
    };

            return climberData;
        }

        private List<ApplicationUser> UsersForTests()
        {

            return new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Id = "652ef9d0-1856-44fc-af36-7027a112f0ee",
                FirstName = "Hristo",
                LastName = "Hristov",
                Email = "john@example.com",
                Age = 25,
                UserType = "Climber"
            },
            new ApplicationUser
            {
                Id = "652ef9d0-1856-44fc-af36-7027a112f0ee",
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "john@example.com",
                Age = 15,
                UserType = "Administrator"
            },
            new ApplicationUser
            {
                Id = "652ef9d0-1856-44fc-af36-7027a112f0ee",
                FirstName = "Pesho",
                LastName = "Petrov",
                Email = "john@example.com",
                Age = 35,
                UserType = "Coach"
            }
        };
        }
    }
}
