namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Common;
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using NUnit.Framework.Internal;
    using WebShopDemo.Core.Data.Common;

    [TestFixture]
    public class AdminServiceTests
    {
        private IAdminService _adminService;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IRepository> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _repositoryMock = new Mock<IRepository>();

            _adminService = new AdminService(
                _userManagerMock.Object,
                _repositoryMock.Object);
        }

        [Test]
        public async Task Test_ActivateTrainingByIdAsync_ReturnsTrue()
        {
            // Arrange
            var trainingId = Guid.NewGuid().ToString();
            var training = new Training { Id = Guid.Parse(trainingId), isActive = false };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(training);

            // Act
            await _adminService.ActivateTrainingByIdAsync(trainingId);

            // Assert
            Assert.IsTrue(training.isActive);
        }
        [Test]
        public async Task Test_ActivateTrainingByIdAsync_TrainingNotFound()
        {
            // Arrange
            var trainingId = Guid.NewGuid().ToString();
             _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync((Training)null);

            // Act and Assert
             Assert.That(async () => await _adminService.ActivateTrainingByIdAsync(trainingId),
                        Throws.Exception, "Object reference not set to an instance of an object.");

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_ActivateTrainingByIdAsync_TrainingAlreadyActive()
        {
            // Arrange
            var trainingId = Guid.NewGuid().ToString();
            var training = new Training { Id = Guid.Parse(trainingId), isActive = true };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(training);

            // Act
            await _adminService.ActivateTrainingByIdAsync(trainingId);

            // Assert
            Assert.IsTrue(training.isActive); 
        }
        [Test]
        public async Task Test_ActivateTrainingByIdAsync_InvalidTrainingId()
        {
            // Arrange
            var invalidTrainingId = "invalid-id";

            // Act and Assert
             Assert.ThrowsAsync<FormatException>(async () =>
                await _adminService.ActivateTrainingByIdAsync(invalidTrainingId));

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_ActivateTrainingByIdAsync_SaveChangesCalled()
        {
            // Arrange
            var trainingId = Guid.NewGuid().ToString();
            var training = new Training { Id = Guid.Parse(trainingId), isActive = false };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(trainingId)))
                           .ReturnsAsync(training);

            // Act
            await _adminService.ActivateTrainingByIdAsync(trainingId);

            // Assert
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task Test_ActivateClimbingTripByIdAsync_Success()
        {
            // Arrange
            var climbingTripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(climbingTripId), IsActive = false };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(climbingTripId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            await _adminService.ActivateClimbingTripByIdAsync(climbingTripId);

            // Assert
            Assert.IsTrue(climbingTrip.IsActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_ActivateClimbingTripByIdAsync_ClimbingTripNotFound()
        {
            // Arrange
            var climbingTripId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(climbingTripId)))
                           .ReturnsAsync((ClimbingTrip)null);

            // Act and Assert
            Assert.That(async () => await _adminService.ActivateClimbingTripByIdAsync(climbingTripId),
                        Throws.Exception, "Object reference not set to an instance of an object.");
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_ActivateClimbingTripByIdAsync_AlreadyActive()
        {
            // Arrange
            var climbingTripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(climbingTripId), IsActive = true };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(climbingTripId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            await _adminService.ActivateClimbingTripByIdAsync(climbingTripId);

            // Assert
            Assert.IsTrue(climbingTrip.IsActive);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_ActivateClimbingTripByIdAsync_SaveChangesFailure()
        {
            // Arrange

            
            var climbingTripId = Guid.NewGuid().ToString();
            var climbingTrip = new ClimbingTrip { Id = Guid.Parse(climbingTripId), IsActive = false };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(climbingTripId)))
                           .ReturnsAsync(climbingTrip);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("SaveChanges failed"));

            // Act and Assert
             Assert.ThrowsAsync<Exception>(async () => await _adminService.ActivateClimbingTripByIdAsync(climbingTripId));
            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_BecomeClimberAsync_Success()
        {
            // Arrange
            var adminUserId = "adminUserId";
            var adminUser = new ApplicationUser { Id = adminUserId };

            _userManagerMock.Setup(manager => manager.FindByIdAsync(adminUserId))
                            .ReturnsAsync(adminUser);

            // Act
            await _adminService.BecomeClimberAsync(adminUserId);

            // Assert
            _userManagerMock.Verify(manager => manager.AddToRoleAsync(adminUser, RoleConstants.Climber), Times.Once);
        }

        [Test]
        public async Task Test_BecomeCoachAsync_Success()
        {
            // Arrange
            var adminUserId = "adminUserId";
            var adminUser = new ApplicationUser { Id = adminUserId };

            _userManagerMock.Setup(manager => manager.FindByIdAsync(adminUserId))
                            .ReturnsAsync(adminUser);

            // Act
            await _adminService.BecomeCoachAsync(adminUserId);

            // Assert
            _userManagerMock.Verify(manager => manager.AddToRoleAsync(adminUser, RoleConstants.Coach), Times.Once);
        }
    }

}
