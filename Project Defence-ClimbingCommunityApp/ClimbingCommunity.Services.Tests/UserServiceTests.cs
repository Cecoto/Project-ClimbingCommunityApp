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
